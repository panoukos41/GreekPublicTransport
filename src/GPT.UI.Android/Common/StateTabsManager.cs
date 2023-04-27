//using Google.Android.Material.Tabs;
//using static Google.Android.Material.Tabs.TabLayoutMediator;

//namespace AndroidX.ViewPager2.Adapter;

//using AndroidX.ViewPager2.Widget;
//using Fragment = AndroidX.Fragment.App.Fragment;

///// <summary>
///// A class used to link the provided <see cref="TabLayout"/> and <see cref="ViewPager"/>.
///// The <see cref="ViewPager2.Adapter"/> will be created automatically.
///// </summary>
//public class StateTabsManager : Java.Lang.Object, ITabConfigurationStrategy
//{
//    private List<(Action<TabLayout.Tab> tab, Fragment fragment)> Tabs { get; set; }

//    /// <summary>
//    /// </summary>
//    public int Count => Adapter == null ? 0 : Adapter.ItemCount;

//    /// <summary>
//    /// Your <see cref="Google.Android.Material.Tabs.TabLayout"/>
//    /// </summary>
//    public TabLayout TabLayout { get; set; }

//    /// <summary>
//    /// Your <see cref="ViewPager2"/>
//    /// </summary>
//    public ViewPager2 ViewPager { get; set; }

//    /// <summary>
//    /// The generated <see cref="StateAdapter"/> that will use the function.
//    /// </summary>
//    public StateAdapter Adapter { get; private set; }

//    /// <summary>
//    /// The generated <see cref="TabLayoutMediator"/>
//    /// </summary>
//    public TabLayoutMediator Mediator { get; private set; }

//    /// <summary>
//    /// Set the <see cref="Google.Android.Material.Tabs.TabLayout"/> property in a fluent manner.
//    /// </summary>
//    /// <param name="tabLayout"></param>
//    /// <returns></returns>
//    public StateTabsManager SetTabLayout(TabLayout tabLayout)
//    {
//        TabLayout = tabLayout;
//        return this;
//    }

//    /// <summary>
//    /// Set the <see cref="global::AndroidX.ViewPager.Widget.ViewPager"/> property in a fluent manner.
//    /// </summary>
//    /// <param name="viewPager"></param>
//    /// <returns></returns>
//    public StateTabsManager SetViewPager(ViewPager2 viewPager)
//    {
//        ViewPager = viewPager;
//        return this;
//    }

//    /// <summary>
//    /// Initializes the Mediator and Adapter objects.
//    /// Sets their properties and calls Attach() on the mediator.
//    /// </summary>
//    /// <param name="fragment">The fragment that host's the adapter.</param>
//    /// <param name="tabs">A list with methods to set tab titles and retrieve content.</param>
//    /// <param name="autoRefresh"></param>
//    public StateTabsManager Attach(Fragment fragment, List<(Action<TabLayout.Tab> tab, Fragment fragment)> tabs, bool autoRefresh = false)
//    {
//        Tabs = tabs;
//        Adapter = new StateAdapter(fragment, Tabs);
//        ViewPager.Adapter = Adapter;

//        // I don't know how autorefresh works so just to be safe :P
//        if (autoRefresh)
//            Mediator = new TabLayoutMediator(TabLayout, ViewPager, autoRefresh, this);
//        else
//            Mediator = new TabLayoutMediator(TabLayout, ViewPager, this);
//        Mediator.Attach();

//        return this;
//    }

//    /// <summary>
//    /// Set the title of the tab at the specified position.
//    /// </summary>
//    /// <param name="tab"></param>
//    /// <param name="position"></param>
//    public void SetTabTitle(TabLayout.Tab tab, int position) =>
//        Tabs[position].tab.Invoke(tab);

//    /// <summary>
//    /// Get's the fragment at the specified position.
//    /// </summary>
//    /// <param name="position"></param>
//    /// <returns></returns>
//    public Fragment GetTabContent(int position) =>
//        Tabs[position].fragment;

//    void ITabConfigurationStrategy.OnConfigureTab(TabLayout.Tab p0, int p1)
//        => SetTabTitle(p0, p1);
//}