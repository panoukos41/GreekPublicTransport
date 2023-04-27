using Android.Views;
using Google.Android.Material.BottomAppBar;
using Google.Android.Material.FloatingActionButton;
using GPT.Abstractions;
using ReactiveUI;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

namespace GPT.UI.Android;

[Activity(MainLauncher = true, Theme = "@style/AppTheme")]
public class Shell : ActivityBase
{
    private readonly NavigationHost host = (NavigationHost)Services.Resolve<INavigationHost>();
    private readonly CompositeDisposable disposables = new();

    private Toolbar Toolbar => FindCachedView<Toolbar>(R.Id.toolbar);
    private BottomAppBar BottomBar => FindCachedView<BottomAppBar>(R.Id.bottomBar);
    private FloatingActionButton BottomBarFab => FindCachedView<FloatingActionButton>(R.Id.bottomBarFab);

    protected override void OnCreate(Bundle? savedInstanceState)
    {
        base.OnCreate(savedInstanceState);
        SetContentView(R.Layout.shell);
        CreateMenu();

        var navDisposables = new CompositeDisposable().DisposeWith(disposables);
        var whenNavigated = host.WhenNavigated(h =>
        {
            navDisposables.Clear();

            if (h.CurrentView is not IViewFor v) return;
            if (v.ViewModel is IViewModelTitle viewModel)
            {
                viewModel
                    .WhenAnyValue(vm => vm.Title)
                    .Throttle(TimeSpan.FromMilliseconds(250))
                    .SubscribeMain(t => Toolbar.Title = t)
                    .DisposeWith(navDisposables);
            }
            if (h.CurrentView is IViewAction(var action))
            {
                BottomBarFab.Tag = action.Name;
                BottomBarFab.SetImageDrawable(Resources!.GetDrawable(action.Icon));
                BottomBarFab.Events().Click.Do(_ =>
                {
                    action.Command?.Invoke().Dispose();
                })
                .Subscribe()
                .DisposeWith(navDisposables);
            }
        })
        .DisposeWith(disposables);

        host.SetFragmentManager(SupportFragmentManager)
            .SetFragmentContainerId(R.Id.fragmentContainer)
            .NavigateToLine(new("1363"));  // 214
                                           //.NavigateToStop(new("60589")); // Byzantino Mouseio
                                           //.NavigateToHome();
    }

    private void CreateMenu()
    {
        if (BottomBar.Menu is null) return;

        var test = BottomBar.Menu.Add("Test")!;
        test.SetIcon(R.Drawable.ic_bookmark);
        test.SetShowAsAction(ShowAsAction.Always);
    }

    #region Android Events

    public override void OnBackPressed()
    {
        if (host.Count <= 1)
        {
            Finish();
            return;
        }
        host.GoBack();
    }

    protected override void OnDestroy()
    {
        Services.Resolve<IStorage>().Checkpoint().Subscribe();
        disposables.Dispose();
        base.OnDestroy();
    }

    #endregion
}
