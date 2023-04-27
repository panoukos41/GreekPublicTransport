using DynamicData;
using DynamicData.Kernel;
using Flurl.Http;
using GPT.Bus;
using ReactiveUI;
using System.ComponentModel;

namespace GPT.UI.Console.Oasa;

public sealed class DownloadCommandOptions : CommandSettings
{
    [Description("Should it download routes? Default = false.")]
    [CommandOption("-r|--routes")]
    [DefaultValue(false)]
    public bool Routes { get; set; }

    [Description("Should it download stops? Default = false. If true routes will be downloaded too.")]
    [CommandOption("-s|--stops")]
    [DefaultValue(false)]
    public bool Stops { get; set; }

    [Description("Should it download shcedules? Default = false.")]
    [CommandOption("-t|--schedules")]
    [DefaultValue(false)]
    public bool Schedules { get; set; }

    // todo: Consider in future to download if necessary.

    //[Description("Should it download timetables? Default = false. If true schedules will be downloaded too.")]
    //[CommandOption("--timetables")]
    //[DefaultValue(false)]
    //public bool Timetables { get; set; }

    public Table RenderTable()
    {
        var table = new Table();

        table.MinimalBorder();
        table.Collapse();

        table.AddColumns("Setting", "Value");
        table.AddRow(nameof(Routes), Routes.ToString());
        table.AddRow(nameof(Stops), Stops.ToString());
        table.AddRow(nameof(Schedules), Schedules.ToString());

        return table;
    }
}

public sealed class DownloadCommand : AsyncCommand<DownloadCommandOptions>
{
    private readonly IBusClient client = Services.Resolve<IBusClient>();
    private readonly IStorage storage = Services.Resolve<IStorage>();

    public override async Task<int> ExecuteAsync(CommandContext ctx, DownloadCommandOptions options)
    {
        AnsiConsole.Write(options.RenderTable());

        var (_, d) = AnsiConsole.Status().Start("[bold]Downloading Oasa data![/]");
        var startTime = DateTime.Now;

        // Get lines
        var lines = await DownloadLines();

        // Get routes.
        if (options.Routes || options.Stops)
        {
            var routes = await DownloadRoutes(lines);

            // Get stops.
            if (options.Stops)
            {
                await DownloadStops(routes);
            }
        }

        // Get schedules
        if (options.Schedules /*|| options.Timetables*/)
        {
            var schedules = await DownloadSchedules(lines);

            //// Get timetables
            //if (options.Timetables)
            //{
            //    await DownloadTimetables(schedules);
            //}
        }

        var totalTime = DateTime.Now - startTime;
        Log($"Finished!", totalTime);

        d.Dispose();
        return 0;
    }

    async Task<IList<Line>> DownloadLines()
    {
        Log("Downloading lines.");

        var lines = new List<Line>(512);

        var elapsed = await client
            .GetLines()
            .RetryWithBackOff<Line[], FlurlHttpException>((ex, i) => TimeSpan.FromSeconds(i))
            .Do(lines.AddRange)
            .Time();

        Log($"Downloaded {lines.Count} lines", elapsed);

        elapsed = await storage
            .Set(lines.ToDictionary(static line => line.Id.Value))
            .Time();

        Log($"Stored {lines.Count} lines", elapsed);

        return lines;
    }

    async Task<IList<Route>> DownloadRoutes(IList<Line> lines)
    {
        Log($"Downloading {lines.Count} route pairs.");

        var routes = new List<Route>(lines.Count * 2);

        var elapsed = await lines
            .Select(line => client
                .GetRoutesForLine(line)
                .RetryWithBackOff<Route[], FlurlHttpException>((ex, i) => TimeSpan.FromSeconds(i)))
            .Merge(maxConcurrent: 5)
            .Do(routes.AddRange)
            .Time();

        Log($"Downloaded {routes.Count:g} routes", elapsed);

        var linesDict = lines.ToDictionary(static line => line.Id.Value);
        var routesDict = routes.DistinctBy(static route => route.Id).ToDictionary(static route => route.Id.Value);

        foreach (var group in routesDict.Values.GroupBy(static x => x.LineId))
        {
            var line = linesDict[group.Key.Value];
            line.RouteIds = line.RouteIds = group.Select(static x => x.Id).ToArray();
        }

        elapsed = await storage.Set(routesDict).Time();

        Log($"Stored {routes.Count:g} routes", elapsed);

        elapsed = await storage.Set(linesDict).Time();

        Log($"Updated {lines.Count:g} lines", elapsed);

        return routes;
    }

    async Task<IList<Schedule>> DownloadSchedules(IList<Line> lines)
    {
        Log($"Downloading {lines.Count} schedule pairs.");

        var schedules = new List<Schedule>(lines.Count * 3);

        var elapsed = await lines
            .Select(line => client
                .GetSchedules(line)
                .RetryWithBackOff<Schedule[]?, FlurlHttpException>((ex, i) => TimeSpan.FromSeconds(i))
                .WhereNotNull())
            .Merge(maxConcurrent: 5)
            .Do(schedules.AddRange)
            .Time();

        Log($"Downloaded {schedules.Count:g} schedules", elapsed);

        var linesDict = lines.ToDictionary(static line => line.Id.Value);
        var schedulesDict = schedules.DistinctBy(static x => x.Id).ToDictionary(static x => x.Id.Value);

        foreach (var group in schedulesDict.Values.GroupBy(static x => x.Id.LineId))
        {
            var line = linesDict[group.Key.Value];
            line.SchedulelIds = group.Select(static x => x.Id).ToArray();
        }

        elapsed = await storage.Set(schedulesDict).Time();

        Log($"Stored {schedules.Count:g} schedules", elapsed);

        elapsed = await storage.Set(linesDict).Time();

        Log($"Updated {lines.Count:g} lines", elapsed);

        return schedules;
    }

    async Task DownloadStops(IList<Route> routes)
    {
        Log($"Downloading a lot of stops for {routes.Count:g} different routes.");

        var stops = new List<Stop>(routes.Count * 3);

        var elapsed = await routes
            .Select(route => client
                .GetStopsForRoute(route)
                .RetryWithBackOff<Stop[], FlurlHttpException>((ex, i) => TimeSpan.FromSeconds(i)))
            .Merge(maxConcurrent: 5)
            .Do(stops.AddRange)
            .Time();

        stops = stops.DistinctBy(static x => x.Id).ToList();

        Log($"Downloaded {stops.Count:g} stops", elapsed);

        //var routesDict = routes.ToDictionary(static route => route.Id.Value);
        var stopsDict = stops.ToDictionary(static stop => stop.Id.Value);

        elapsed = await storage.Set(stopsDict).Time();

        Log($"Stored {stops.Count:g} stops", elapsed);

        //elapsed = await storage.Set(routesDict).Time();

        //Log($"Updated {routes.Count:g} routes", elapsed);
    }

    static void Log(string message)
        => AnsiConsole.MarkupLine($"[bold gray]Log:[/]  {message}");

    static void Log(string message, TimeSpan timeSpan)
        => AnsiConsole.MarkupLine($"[bold gray]Log:[/]  {message}  [yellow]{timeSpan.TotalMilliseconds:g}ms [/]");
}

public static class Extensions
{
    public static IObservable<TimeSpan> Time<T>(this IObservable<T> observable)
        => Observable.Create<TimeSpan>(o =>
        {
            var now = DateTime.Now;

            var sub = observable.Subscribe(
                onNext: _ => { },
                onCompleted: () =>
                {
                    var elapsed = DateTime.Now - now;
                    o.OnNext(elapsed);
                    o.OnCompleted();
                });

            return sub;
        });
}
