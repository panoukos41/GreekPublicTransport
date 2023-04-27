using AndroidX.RecyclerView.Widget;
using Google.Android.Material.FloatingActionButton;
using Google.Android.Material.TextView;
using GPT.Abstractions;
using GPT.UI.Android.Common;
using GPT.UI.Android.Components;
using ReactiveUI;
using System.Globalization;

namespace GPT.Bus;

using View = Android.Views.View;

public class LineDetailsView : FragmentBase<LineDetailsViewModel>
{
    private readonly SourceList<TimeWrapper> _times = new();

    public LineDetailsView() : base(R.Layout.linedetailsview)
    {
    }

    private BottomSheetMenu<IId>? bottomMenu;
    private ExtendedFloatingActionButton? fab;
    private MaterialTextView? textView;
    private RecyclerView? recyclerView;

    public override void OnCreateView(View view, Bundle? savedInstanceState, CompositeDisposable disposables)
    {
        var context = view.Context!;

        bottomMenu = new();
        recyclerView = view.FindViewById<RecyclerView>(R.Id.recyclerView)!;
        textView = view.FindViewById<MaterialTextView>(R.Id.textView)!;
        fab = view.FindViewById<ExtendedFloatingActionButton>(R.Id.fab)!;

        recyclerView.Setup(context);
        recyclerView.ConnectFab(fab, false);

        fab.Events().Click.Subscribe(_ => bottomMenu.ShowNow(ChildFragmentManager, "bottomMenu"));

        bottomMenu.Clear();

        // Set bottom menu items
        var routesGroup = new MenuGroup(0, "Routes");
        var routesObs = ViewModel
            .WhenAnyValue(vm => vm.Routes)
            .WhereNotNull()
            .SelectMany(static routes => routes)
            .Subscribe(route => bottomMenu.Add(new(route.Id, route.Description, routesGroup)));

        var schedulesGroup = new MenuGroup(1, "Schedules");
        var schedulesObs = ViewModel
            .WhenAnyValue(vm => vm.Schedules)
            .WhereNotNull()
            .SelectMany(static schedules => schedules)
            .Subscribe(schedule => bottomMenu.Add(new(schedule.Id, schedule.Description, schedulesGroup)));

        var stopAdapter = ViewModel!.Stops.ToRecyclerAdapter(
            resource: R.Layout.linedetailsview_stop,
            onBind: (v, item, d) => v.FindViewById<TextView>(R.Id.textView)!.Text = item.Description,
            clickedItem: stop => Services.Resolve<INavigationHost>().NavigateToStop(stop.Id));

        var timeAdapter = _times
            .Connect()
            .ToRecyclerAdapter(R.Layout.linedetailsview_time, (v, item, d) => ((TextView)v).Text = item);

        // Act when the selected item changes.
        ViewModel
            .WhenAnyValue(vm => vm.Selected)
            .WhereNotNull()
            .Subscribe(selected =>
            {
                bottomMenu.SetEnableToAll(true);
                if (selected.TryPickT0(out Route route, out Schedule schedule))
                {
                    textView.Text = null;
                    recyclerView.SetAdapter(stopAdapter);
                    fab.Text = route.Description;
                    bottomMenu[route.Id].Enabled = false;
                }
                else
                {
                    textView.Text = schedule.Description;
                    recyclerView.SetAdapter(timeAdapter);
                    fab.Text = schedule.Description;

                    bottomMenu[schedule.Id].Enabled = false;

                    var start = schedule.FromStartingPoint;
                    var terminal = schedule.FromTerminalPoint;

                    // todo: TextView should be description and display start/endpoint while
                    // the recycler should contain one value from each (start - end)
                    // so it lines up perfectly when scrolling.

                    if (start.Length > 0)
                    {
                        if (terminal.Length == 0)
                        {
                            _times.Clear();
                            _times.AddRange(Result(start));
                            return;
                        }
                    }
                    _times.Clear();
                    _times.AddRange(Result(terminal));

                    static IEnumerable<TimeWrapper> Result(TimeOnly[] times) => times
                        .OrderBy(static time => time)
                        .Select(static time => new TimeWrapper(time));
                }
            });

        // Set the selected item based on menu click.
        bottomMenu.WhenItemClick.Subscribe(item =>
        {
            var group = item.Group;
            var value = item.Key.Value;

            ViewModel.Selected = group == routesGroup
                ? ViewModel.Routes!.First(x => x.Id.Value == value)
                : ViewModel.Schedules!.First(x => x.Id.Value == value);
        });
    }

    private class TimeWrapper
    {
        public TimeWrapper(TimeOnly time) => Time = time.ToString(CultureInfo.InvariantCulture.DateTimeFormat);

        public string Time { get; }

        public static implicit operator string(TimeWrapper self) => self.Time;
    }
}
