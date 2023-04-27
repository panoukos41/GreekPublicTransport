using P41.Navigation;

namespace GPT.UI.Console;

public class ConsoleServiceProvider : ServiceProvider
{
    protected override string DatabaseDirectory { get; } = Environment.CurrentDirectory;

    protected override INavigationHost GetNavigationHost()
    {
        throw new NotImplementedException();
    }
}
