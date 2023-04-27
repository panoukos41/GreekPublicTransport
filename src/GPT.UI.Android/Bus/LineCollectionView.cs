using Android.Views;
using AndroidX.RecyclerView.Widget;
using Google.Android.Material.Divider;
using Google.Android.Material.TextField;
using GPT.Abstractions;
using GPT.UI.Android.Common;
using ReactiveUI;

namespace GPT.Bus;

using View = View;

// todo: Test
public class LineCollectionView : FragmentBase<LineCollectionViewModel>, IViewAction
{
    public LineCollectionView() : base(R.Layout.linecollectionview)
    {
    }

    private RecyclerAdapter<Line>? _lineAdapter;

    public ViewAction Action { get; } = new()
    {
        Name = "Search",
        Icon = R.Drawable.ic_search,
        Command = null
    };

    public override void OnCreateView(View view, Bundle? savedInstanceState, CompositeDisposable disposables)
    {
        var recycler = view.FindViewById<RecyclerView>(R.Id.recyclerView)!;
        var searchField = view.FindViewById<TextInputLayout>(R.Id.searchField)!;

        searchField.EditText.Events()
            .TextChanged
            .BindTo(ViewModel, vm => vm.Search)
            .DisposeWith(disposables);

        Action.Command = ReactiveCommand.Create(() =>
        {
            searchField.Visibility = ViewStates.Visible;
        });

        _lineAdapter ??= ViewModel!.Lines.ToRecyclerAdapter(
            resource: R.Layout.linecollectionview_item,
            onBind: (v, line, d) =>
            {
                v.FindViewById<TextView>(R.Id.title)!.Text = line.Name;
                v.FindViewById<TextView>(R.Id.description)!.Text = line.Description;
            },
            clickedItem: line => Services.Resolve<INavigationHost>().NavigateToLine(line.Id));

        recycler.Setup(view);
        recycler.AddItemDecoration(new MaterialDividerItemDecoration(Context!, LinearLayoutManager.Vertical)
        {
            LastItemDecorated = false
        });
        recycler.SetAdapter(_lineAdapter);
    }
}
