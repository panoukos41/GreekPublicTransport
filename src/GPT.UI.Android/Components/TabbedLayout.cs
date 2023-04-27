//using Android.Content;
//using Android.Util;
//using AndroidX.CoordinatorLayout.Widget;
//using Fragment = AndroidX.Fragment.App.Fragment;

//namespace GPT.UI.Android.Components;

//public delegate void TabbedLayoutItemDelegate(global::Android.Views.View view, Bundle? savedInstanceState);

//public class TabbedLayout : CoordinatorLayout
//{
//    // MaterialButtonToggleGroup: Holds buttons.
//    // FragmentContainerView    : The tabs.

//    // Provide group position control.
//    // Should use from xml.
//    // Buttons added from xml or code.
//    // Provide events for selected button etc.

//    public TabbedLayoutItemDelegate? this[int index]
//    {
//        get => tabs[index].Delegate;
//        set => tabs[index].Delegate = value;
//    }

//    public TabbedLayout(Context context)
//        : this(context, null)
//    {
//    }

//    public TabbedLayout(Context context, IAttributeSet? attrs)
//        : base(context, attrs)
//    {
//        // todo: Custom logic goes here.
//    }

//    private List<TabGroup> tabs;

//    private class TabGroup
//    {
//        public TabbedLayoutItem Item { get; set; }

//        public Fragment? Fragment { get; set; }

//        public TabbedLayoutItemDelegate? Delegate { get; set; }
//    }
//}

//public class TabbedLayoutItem
//{
//    public TabbedLayoutItem(string title, int layout)
//    {
//        Title = title;
//        Layout = layout;
//    }

//    public string Title { get; }

//    public int Layout { get; }
//}
