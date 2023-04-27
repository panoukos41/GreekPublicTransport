//using Java.Lang;

//namespace AndroidX.Fragment.App;

///// <summary>
///// An <see cref="FragmentPagerAdapter"/> that provides the 2 important methods out of the box.
///// You can provide the methods or implement the Interface <see cref="IPagerTabs"/>
///// </summary>
//public class PagerAdapter : FragmentPagerAdapter
//{
//    /// <summary>
//    /// Item count.
//    /// </summary>
//    public override int Count => Tabs == null ? 0 : Tabs.Count;

//    private List<(string title, Fragment fragment)> Tabs { get; }

//    /// <summary>
//    /// Create a pager adapter that hosts a fixed number of fragments.
//    /// </summary>
//    /// <param name="fm">The <see cref="FragmentManager"/> that will host the tabs.</param>
//    /// <param name="tabs">A list with methods to retrieve tab titles and content.</param>
//    public PagerAdapter(FragmentManager fm, List<(string title, Fragment fragment)> tabs)
//        : base(fm, BehaviorResumeOnlyCurrentFragment)
//    {
//        Tabs = tabs;
//    }

//    /// <summary>
//    /// Get the fragment for this position.
//    /// </summary>
//    public override Fragment GetItem(int position) =>
//        Tabs[position].fragment;

//    /// <summary>
//    /// Get the title for this position.
//    /// </summary>
//    public override ICharSequence GetPageTitleFormatted(int position) =>
//        new Java.Lang.String(Tabs[position].title);
//}
