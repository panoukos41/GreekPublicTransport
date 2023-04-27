//using Google.Android.Material.Tabs;

//namespace AndroidX.Fragment.App;

//using ViewPager = AndroidX.ViewPager.Widget.ViewPager;

///// <summary>
///// A class used to link the provided <see cref="TabLayout"/> and <see cref="ViewPager"/>.
///// The <see cref="ViewPager.Adapter"/> will be created automatically.
///// </summary>
//public class PagerTabsManager
//{
//    private List<(string title, Fragment fragment)> Tabs { get; set; }

//    /// <summary>
//    /// </summary>
//    public int Count => Adapter == null ? 0 : Adapter.Count;

//    /// <summary>
//    /// Your <see cref="Google.Android.Material.Tabs.TabLayout"/>
//    /// </summary>
//    public TabLayout TabLayout { get; set; }

//    /// <summary>
//    /// Your <see cref="global::AndroidX.ViewPager.Widget.ViewPager"/>
//    /// </summary>
//    public ViewPager ViewPager { get; set; }

//    /// <summary>
//    /// The generated <see cref="PagerAdapter"/>.
//    /// </summary>
//    public PagerAdapter Adapter { get; private set; }

//    /// <summary>
//    /// Set the <see cref="Google.Android.Material.Tabs.TabLayout"/> property in a fluent manner.
//    /// </summary>
//    /// <param name="tabLayout"></param>
//    /// <returns></returns>
//    public PagerTabsManager SetTabLayout(TabLayout tabLayout)
//    {
//        TabLayout = tabLayout;
//        return this;
//    }

//    /// <summary>
//    /// Set the <see cref="ViewPager"/> property in a fluent manner.
//    /// </summary>
//    /// <param name="viewPager"></param>
//    /// <returns></returns>
//    public PagerTabsManager SetViewPager(ViewPager viewPager)
//    {
//        ViewPager = viewPager;
//        return this;
//    }

//    /// <summary>
//    /// Generates a <see cref="PagerAdapter"/> and set's up the TabLayout with the ViewPager
//    /// by calling <see cref="TabLayout.SetupWithViewPager(ViewPager)"/>.
//    /// </summary>
//    /// <param name="fm">The <see cref="FragmentManager"/> to be used by the adapter.</param>
//    /// <param name="tabs">A list with methods to retrieve tab titles and content.</param>
//    /// <param name="autoRefresh"></param>
//    public PagerTabsManager Attach(FragmentManager fm, List<(string title, Fragment fragment)> tabs, bool autoRefresh = false)
//    {
//        Tabs = tabs;
//        Adapter = new PagerAdapter(fm, Tabs);
//        ViewPager.Adapter = Adapter;

//        // I don't know how autorefresh works so just to be safe :P
//        if (autoRefresh)
//            TabLayout.SetupWithViewPager(ViewPager, autoRefresh);
//        else
//            TabLayout.SetupWithViewPager(ViewPager);
//        return this;
//    }

//    /// <summary>
//    /// Get the tab's content.
//    /// </summary>
//    public Fragment GetTabContent(int position) =>
//        Tabs[position].fragment;

//    /// <summary>
//    /// Get the tab's title.
//    /// </summary>
//    public string GetTabTitle(int position) =>
//        Tabs[position].title;
//}