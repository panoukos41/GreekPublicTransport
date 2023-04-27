namespace AndroidX.AppCompat.Widget;

using AndroidX.AppCompat.App;

/// <summary>
/// Extension methods to use with <see cref="AppCompatActivity"/>.
/// </summary>
public static class AppCompatActivityExtensions
{
    /// <summary>
    /// Set the Toolbar using the SetSupportActionBar(Toolbar).
    /// </summary>
    /// <param name="activity">The activity on which the Toolbar will be set.</param>
    /// <param name="Toolbar">The Toolbar to set.</param>
    public static void SetToolbar(this AppCompatActivity activity, Toolbar Toolbar) =>
        activity.SetSupportActionBar(Toolbar);

    /// <summary>
    /// Set the Toolbar and connect it with a DrawerLayout. It already calls <see cref="SetToolbar(AppCompatActivity, Toolbar)"/>.
    /// </summary>
    /// <param name="activity">The activity on which the Toolbar will be set.</param>
    /// <param name="Toolbar">The Toolbar to set.</param>
    /// <param name="drawerLayout">The DrawerLayout to connect with the Toolbar.</param>
    /// <param name="openDrawerContentDescRes">A Resource that will be used to describe the opened state of the DrawerLayout.</param>
    /// <param name="closeDrawerContentDescRes">A Resource that will be used to describe the closed state of the DrawerLayout.</param>
    /// <returns>The ActionBarDrawerToggle that was created to connect the Toolbar and the DrawerLayout</returns>
    public static ActionBarDrawerToggle SetToolbarWithDrawer(
        this AppCompatActivity activity,
        Toolbar Toolbar, DrawerLayout.Widget.DrawerLayout
        drawerLayout,
        int openDrawerContentDescRes,
        int closeDrawerContentDescRes)
    {
        activity.SetToolbar(Toolbar);

        var toggle = new ActionBarDrawerToggle(
            activity,
            drawerLayout,
            Toolbar,
            openDrawerContentDescRes,
            closeDrawerContentDescRes);

        drawerLayout.AddDrawerListener(toggle);
        toggle.SyncState();
        return toggle;
    }
}
