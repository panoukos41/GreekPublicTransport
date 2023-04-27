namespace GPT.UI.Console.Oasa;

public class CountCommand : AsyncCommand
{
    public override async Task<int> ExecuteAsync(CommandContext context)
    {
        //var storage = App.Resolve<IStorage>();

        var lines = 0;
        var schedules = 0;
        var routes = 0;
        var stops = 0;

        await Task.Delay(0);

        AnsiConsole.MarkupLine("[bold]Counting Oasa data[/]");

        AnsiConsole.Write(new Table()
            .AddColumn(new TableColumn("[yellow]Collection[/]").RightAligned())
            .AddColumn(new TableColumn("[yellow]Count[/]").LeftAligned())
            .AddRow("Lines", lines.ToString())
            .AddRow("Schedules", schedules.ToString())
            .AddRow("Routes", routes.ToString())
            .AddRow("Stops", stops.ToString())
            .Border(TableBorder.Simple)
        );

        return 0;
    }
}
