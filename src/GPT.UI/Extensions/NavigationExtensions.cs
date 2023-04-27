using GPT.Bus;

namespace P41.Navigation;

public static class NavigationExtensions
{
    public static void NavigateToHome(this INavigationHost host)
        => host.Navigate("oasa/");

    public static void NavigateToLine(this INavigationHost host, LineId id)
        => host.Navigate($"oasa/line/{id}");

    public static void NavigateToStop(this INavigationHost host, StopId id)
        => host.Navigate($"oasa/stop/{id}");
}
