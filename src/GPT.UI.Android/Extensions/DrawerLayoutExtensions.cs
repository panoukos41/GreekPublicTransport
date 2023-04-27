namespace AndroidX.DrawerLayout.Widget;

using AndroidX.Core.View;

/// <summary>
/// Extension methods to use with AndroidX's <see cref="DrawerLayout"/>
/// </summary>
public static class DrawerLayoutExtensions
{
    /// <summary>
    /// Will try to close the DrawerLayout using CloseDrawers().
    /// If the drawer is null it will just return false.
    /// </summary>
    /// <returns>True if successful false otherwise.</returns>
    public static bool TryCloseAll(this DrawerLayout drawer)
    {
        if (drawer != null
            && (drawer.IsDrawerOpen(GravityCompat.Start) || drawer.IsDrawerOpen(GravityCompat.End)))
        {
            drawer.CloseDrawers();
            return true;
        }
        return false;
    }
}
