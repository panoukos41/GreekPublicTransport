global using Spectre.Console;
global using Spectre.Console.Cli;
global using System;
global using System.Linq;
global using System.Reactive.Linq;
global using System.Threading.Tasks;
using GPT.Bus;
using GPT.UI.Console;
using GPT.UI.Console.Oasa;

Services.Initialize(new ConsoleServiceProvider());

var app = new CommandApp();

app.Configure(config =>
{
    config.AddBranch("oasa", oasa => oasa.AddCommand<DownloadCommand>("download"));
});

await app.RunAsync(args);

Services.Dispose();

return;

LineCollectionViewModel HomeVm()
{
    var homeVm = Services.Resolve<LineCollectionViewModel>();

    return homeVm;
}

LineDetailsViewModel LineDetainsVm()
{
    var lineVm = Services.Resolve<LineDetailsViewModel>();

    lineVm.Navigator.NavigatedTo(new("line/oasa/1363"));
    return lineVm;
}

StopDetailsViewModel StopDetailsVm()
{
    var stopVm = Services.Resolve<StopDetailsViewModel>();

    stopVm.Navigator.NavigatedTo(new("stop/60589"));

    return stopVm;
}

//var homeVm = HomeVm();
var lineVm = LineDetainsVm();
//var stopVm = StopDetailsVm();

HoldTillEnter();

Services.Dispose();

Console.WriteLine("End");

// returns false when execution is resumed.
bool HoldTillEnter(string? message = null)
{
    Console.WriteLine(message ?? "Press enter to continue");
read:
    if (Console.ReadKey().Key is not ConsoleKey.Enter)
        goto read;

    return false;
}
