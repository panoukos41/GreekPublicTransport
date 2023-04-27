using P41.Navigation;

namespace GPT.Bus;

public class LineDetailsViewModel : ViewModelBase, IViewModelTitle
{
    private readonly SourceList<Stop> _stops = new();

    public LineDetailsViewModel(IBusClient client, IStorage storage)
    {
        // Line
        GetLine = ReactiveCommand.CreateFromObservable<string, Line>(
            canExecute: this.WhenAnyValue(x => x.Line).IsNull(),
            execute: id => storage
                .FindOrGet(id, () => client.GetLine(new(id))));

        GetLine.Subscribe(line =>
        {
            Line = line;
            Title = $"{line!.Name}: {line!.Description}";
            GetRoutes!.Invoke(line);
            GetSchedules!.Invoke(line);
        });

        // Routes
        GetRoutes = ReactiveCommand.CreateFromObservable<Line, Route[]>(
            line => storage
            .FindOrGet(line.RouteIds, () => client.GetRoutesForLine(line!))
            .WhenCondition(line.RouteIds.Length is 0, routes =>
            {
                line.RouteIds = routes.Select(static r => r.Id).ToArray();
                line.Store(storage);
            }));

        GetRoutes.Subscribe(routes =>
        {
            Routes = routes;
            Selected = routes[0];
        });

        // Stops
        GetStops = ReactiveCommand.CreateFromObservable<Route, Stop[]>(
            route => storage
            .FindOrGet(route.StopIds, () => client.GetStopsForRoute(route!))
            .WhenCondition(route.StopIds.Length is 0, stops =>
            {
                route.StopIds = stops.Select(static s => s.Id).ToArray();
                route.Store(storage);
            }));

        GetStops.Subscribe(stops =>
        {
            _stops.Clear();
            _stops.AddRange(stops);
        });

        // Schedules
        GetSchedules = ReactiveCommand.CreateFromObservable<Line, Schedule[]>(
            line => storage
            .FindOrGet(line.SchedulelIds, () => client.GetSchedules(line!))
            .WhenCondition(line.SchedulelIds.Length is 0, schedules =>
            {
                line.SchedulelIds = schedules.Select(static s => s.Id).ToArray();
                line.Store(storage);
            }));

        GetSchedules.Subscribe(
            schedules => Schedules = schedules);

        // Get Routes when selection changes.
        this.WhenAnyValue(x => x.Selected)
            .SelectT0()
            .InvokeCommand(GetStops);

        this.WhenNavigatedTo((r, d) =>
        {
            GetLine.Invoke(r.Segments[2]).DisposeWith(d); // 2 => Id
        });

        Observable.Merge(new[]
        {
            GetLine.ThrownExceptions,
            GetRoutes.ThrownExceptions,
            GetSchedules.ThrownExceptions,
            GetStops.ThrownExceptions,
        })
        .Subscribe();
    }

    public ReactiveCommand<string, Line> GetLine { get; }

    public ReactiveCommand<Line, Route[]> GetRoutes { get; }

    public ReactiveCommand<Route, Stop[]> GetStops { get; }

    public ReactiveCommand<Line, Schedule[]> GetSchedules { get; }

    [Reactive]
    public OneOf<Route, Schedule> Selected { get; set; }

    [Reactive]
    public Line? Line { get; private set; }

    [Reactive]
    public Route[]? Routes { get; private set; }

    [Reactive]
    public Schedule[]? Schedules { get; private set; }

    [Reactive]
    public string? Title { get; private set; }

    public IObservable<IChangeSet<Stop>> Stops => _stops
        .Connect()
        .ObserveOnMain()
        .RefCount();
}
