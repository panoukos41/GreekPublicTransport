using Android.Content;
using Android.Views;
using Google.Android.Material.FloatingActionButton;

namespace AndroidX.RecyclerView.Widget;

public static class RecyclerViewExtensions
{
    private const int ScrollChange = 20;

    private static void ExecuteY(ref bool visible, int dy, Action hide, Action show)
    {
        if (visible && (dy > ScrollChange))
        {
            hide();
            visible = false;
            return;
        }
        if (!visible && (dy < -ScrollChange))
        {
            show();
            visible = true;
        }
    }

    /// <summary>
    /// Setup a recycler view by calling:
    /// <b>SetLayoutManager(LinearLayoutManager)</b> and <b>SetItemAnimator(null)</b>
    /// </summary>
    /// <param name="recyclerView">The <see cref="RecyclerView"/> to setup</param>
    /// <param name="view">The this recycler belongs to.</param>
    public static void Setup(this RecyclerView recyclerView, View view) => Setup(recyclerView, view.Context!);

    public static void Setup(this RecyclerView recyclerView, Context context)
    {
        recyclerView.SetLayoutManager(new LinearLayoutManager(context));
        recyclerView.SetItemAnimator(null);
    }

    public static IObservable<(int dx, int dy)> WhenScroll(this RecyclerView recyclerView)
        => recyclerView
            .Events()
            .ScrollChange
            .Select(static args => (args.ScrollX - args.OldScrollX, args.ScrollY - args.OldScrollY));

    public static IDisposable ConnectFab(this RecyclerView recyclerView, FloatingActionButton? fab)
    {
        if (fab is null) return Disposable.Empty;

        var visible = true;

        return recyclerView
            .WhenScroll()
            .Subscribe(change => ExecuteY(ref visible, change.dy, fab.Hide, fab.Show));
    }

    public static IDisposable ConnectFab(this RecyclerView recyclerView, ExtendedFloatingActionButton? fab, bool showHide = false)
    {
        if (fab is null) return Disposable.Empty;

        var visible = true;
        Action hide = showHide ? fab.Hide : fab.Shrink;
        Action show = showHide ? fab.Show : fab.Extend;

        return recyclerView
            .WhenScroll()
            .Subscribe(change => ExecuteY(ref visible, change.dy, hide, show));
    }

    public static IDisposable ConnectToolbar(this RecyclerView recyclerView, Toolbar toolbar)
    {
        // todo: Implement toolbar support.
        throw new NotImplementedException();
    }
}
