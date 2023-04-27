using Android.Views;
using AndroidX.RecyclerView.Widget;
using Google.Android.Material.FloatingActionButton;

namespace GPT.UI.Android.Common;

using Fragment = AndroidX.Fragment.App.Fragment;
using Toolbar = AndroidX.AppCompat.Widget.Toolbar;

public class RecyclerFragment : Fragment
{
    private readonly Subject<(int dx, int dy)> _whenScroll = new();

    private RecyclerView.Adapter? _adapter;
    private RecyclerView.LayoutManager? _layoutManager;
    private RecyclerView.ItemAnimator? _itemAnimator;
    private RecyclerView.ItemDecoration? _itemDecoration;

    public override View? OnCreateView(LayoutInflater inflater, ViewGroup? container, Bundle? savedInstanceState)
    {
        var context = inflater.Context!;

        if (Recycler is { }) return Recycler;

        Recycler ??= new RecyclerView(context)
        {
            LayoutParameters = new(ViewGroup.LayoutParams.MatchParent, ViewGroup.LayoutParams.MatchParent)
        };

        _layoutManager = new LinearLayoutManager(context);

        Recycler.SetLayoutManager(LayoutManager);
        Recycler.SetItemAnimator(ItemAnimator);
        Recycler.SetAdapter(Adapter);

        Recycler.Events()
            .ScrollChange
            .Select(static args => (args.ScrollX - args.OldScrollX, args.ScrollY - args.OldScrollY))
            .Subscribe(_whenScroll);

        return Recycler;
    }

    public IObservable<(int dx, int dy)> WhenScroll => _whenScroll;

    public RecyclerView? Recycler { get; private set; }

    public RecyclerView.Adapter? Adapter
    {
        get => _adapter;
        set
        {
            _adapter = value;
            Recycler?.SetAdapter(_adapter);
        }
    }

    public RecyclerView.LayoutManager? LayoutManager
    {
        get => _layoutManager;
        set
        {
            _layoutManager = value;
            Recycler?.SetLayoutManager(_layoutManager);
        }
    }

    public RecyclerView.ItemAnimator? ItemAnimator
    {
        get => _itemAnimator;
        set
        {
            _itemAnimator = value;
            Recycler?.SetItemAnimator(_itemAnimator);
        }
    }

    public RecyclerView.ItemDecoration? ItemDecoration
    {
        get => _itemDecoration;
        set
        {
            _itemDecoration = value;
            if (Recycler is { } && _itemDecoration is { })
            {
                Recycler.InvalidateItemDecorations();
                Recycler.AddItemDecoration(_itemDecoration);
            }
        }
    }

    //public void setEmptyView(int layout)
    //public void setEmptyViewAnimations(int enterAnimation, int exitAnimation)
}

public static class RecyclerFragmentMixins
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

    public static IDisposable ConnectFab(this RecyclerFragment fragment, FloatingActionButton? fab)
    {
        if (fab is null) return Disposable.Empty;

        var visible = true;

        return fragment.WhenScroll.Subscribe(change => ExecuteY(ref visible, change.dy, fab.Hide, fab.Show));
    }

    public static IDisposable ConnectFab(this RecyclerFragment fragment, ExtendedFloatingActionButton? fab, bool showHide = false)
    {
        if (fab is null) return Disposable.Empty;

        var visible = true;
        Action hide = showHide ? fab.Hide : fab.Shrink;
        Action show = showHide ? fab.Show : fab.Extend;

        return fragment.WhenScroll.Subscribe(change => ExecuteY(ref visible, change.dy, hide, show));
    }

    public static IDisposable ConnectToolbar(this RecyclerFragment fragment, Toolbar toolbar)
    {
        // todo: Implement toolbar support.
        throw new NotImplementedException();
    }
}
