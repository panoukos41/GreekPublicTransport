//using Android.Content;
//using Android.OS;
//using Android.Views;
//using Android.Widget;
//using AndroidX.AppCompat.App;
//using AndroidX.Fragment.App;
//using AndroidX.Preference;
//using System;

//namespace Bia.Droid.Utils;

//public abstract class PreferenceActivityCompat : AppCompatActivity
//{
//    #region Properties/Fields

//    private SettingsFragment settingsFragment;

//    public PreferenceManager PreferenceManager => settingsFragment.PreferenceManager;

//    public ISharedPreferences SharedPreferences => PreferenceManager.SharedPreferences;

//    #endregion

//    protected override void OnCreate(Bundle savedInstanceState)
//    {
//        base.OnCreate(savedInstanceState);

//        var toolbar = new AndroidX.AppCompat.Widget.Toolbar(this);
//        SetSupportActionBar(toolbar);
//        SupportActionBar.SetDisplayHomeAsUpEnabled(true);

//        var content = new FragmentContainerView(this)
//        {
//            Id = View.GenerateViewId(),
//            LayoutParameters = new ViewGroup.LayoutParams(
//                ViewGroup.LayoutParams.MatchParent,
//                ViewGroup.LayoutParams.MatchParent)
//        };
//        var layout = new LinearLayout(this)
//        {
//            Orientation = Orientation.Vertical,
//            LayoutParameters = new ViewGroup.LayoutParams(
//                ViewGroup.LayoutParams.MatchParent,
//                ViewGroup.LayoutParams.MatchParent)
//        };
//        layout.AddView(toolbar);
//        layout.AddView(content);
//        SetContentView(layout);

//        settingsFragment = SettingsFragment.Create(OnCreatePreferences);
//        SupportFragmentManager
//            .BeginTransaction()
//            .Replace(content.Id, settingsFragment)
//            .Commit();
//    }

//    #region Methods

//    public abstract void OnCreatePreferences(Context context, PreferenceScreen screen, Bundle savedInstanceState, string rootKey);

//    public void ClearPreferences()
//    {
//        var edit = SharedPreferences.Edit();
//        edit.Clear();
//        edit.Commit();
//    }

//    #endregion

//    #region Helper class

//    private class SettingsFragment : PreferenceFragmentCompat
//    {
//        private Action<Context, PreferenceScreen, Bundle, string> CreatePreferences { get; set; }

//        private Context context;
//        private PreferenceScreen screen;

//        public override void OnCreatePreferences(Bundle savedInstanceState, string rootKey)
//        {
//            context = PreferenceManager.Context;
//            screen = PreferenceManager.CreatePreferenceScreen(context);

//            CreatePreferences?.Invoke(context, screen, savedInstanceState, rootKey);

//            PreferenceScreen = screen;
//        }

//        public static SettingsFragment Create(Action<Context, PreferenceScreen, Bundle, string> onCreatePreferences) => new SettingsFragment
//        {
//            CreatePreferences = onCreatePreferences
//        };
//    }

//    #endregion
//}
