namespace GPT.UI.Android.Extensions;

using Fragment = AndroidX.Fragment.App.Fragment;
using FragmentManager = AndroidX.Fragment.App.FragmentManager;

public static class FragmentExtensions
{
    public static void Show(this FragmentManager manager, int containerId, Fragment fragment, string? tag = null)
        => manager
            .BeginTransaction()
            .Replace(containerId, fragment, tag)
            .Commit();

    public static void Show(this FragmentManager manager, int containerId, Fragment fragment, int enter, int exit)
        => manager
            .BeginTransaction()
            .SetCustomAnimations(enter, exit)
            .Replace(containerId, fragment)
            .Commit();

    public static void Show(this FragmentManager manager, int containerId, Fragment fragment, int enter, int exit, int popEnter, int popExit)
        => manager
            .BeginTransaction()
            .SetCustomAnimations(enter, exit, popEnter, popExit)
            .Replace(containerId, fragment)
            .Commit();
}
