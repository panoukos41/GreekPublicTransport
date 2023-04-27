using Android.Views;
using AndroidX.RecyclerView.Widget;
using Google.Android.Material.Divider;
using GPT.Abstractions;
using GPT.UI.Android.Common;
using ReactiveUI;

namespace GPT.Bus;

using static GPT.Bus.StopDetailsViewModel;

public class StopDetailsView : FragmentBase<StopDetailsViewModel>
{
    public StopDetailsView() : base(R.Layout.stopdetailsview)
    {
    }

    private RecyclerAdapter<RouteViewModel>? routeAdapter;

    public override void OnCreateView(View view, Bundle? savedInstanceState, CompositeDisposable disposables)
    {
        routeAdapter ??= ViewModel!.Routes.ToRecyclerAdapter(
            resource: R.Layout.stopdetailsview_route,
            onBind: (v, item, d) =>
            {
                var view = v.FindViewById<TextView>(R.Id.textView)!;
                var name = $"{item.Route.Name}: {item.Route.Description}";

                item.WhenAnyValue(x => x.Arrival)
                    .Subscribe(arrival => view.Text = $"{name}\n{arrival?.ToString() ?? RouteArrival.String}")
                    .DisposeWith(d);
            },
            clickedItem: item => Services.Resolve<INavigationHost>().NavigateToLine(item!.Route.LineId));

        var recycler = view.FindViewById<RecyclerView>(R.Id.routesView)!;
        recycler.Setup(view);
        recycler.AddItemDecoration(new MaterialDividerItemDecoration(Context!, LinearLayoutManager.Vertical)
        {
            LastItemDecorated = false
        });
        recycler.SetAdapter(routeAdapter);
    }
}
