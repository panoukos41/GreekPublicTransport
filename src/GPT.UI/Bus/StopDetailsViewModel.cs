using P41.Navigation;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;

namespace GPT.Bus;

public class StopDetailsViewModel : ViewModelBase, IViewModelTitle
{
    private readonly SourceList<RouteViewModel> _routes = new();
    private readonly Subject<Unit> _routesReSort = new();

    public StopDetailsViewModel(IBusClient client, IStorage storage)
    {
        // Stop
        GetStop = ReactiveCommand.CreateFromObservable<string, Stop>(
            canExecute: this.WhenAnyValue(x => x.Stop).IsNull(),
            execute: id => storage
                .FindOrGet(id, () => client.GetStop(new(id))));

        GetStop.Subscribe(stop =>
        {
            Stop = stop;
            Title = stop.Description;
            GetRoutes!.Invoke(stop);
        });

        // Routes
        GetRoutes = ReactiveCommand.CreateFromObservable<Stop, Route[]>(
            stop => storage
            .FindOrGet(stop.RouteIds, () => client.GetRoutesForStop(stop!))
            .WhenCondition(stop.RouteIds.Length is 0, routes =>
            {
                stop.RouteIds = routes.Select(r => r.Id).ToArray();
                stop.Store(storage);
            }));

        GetRoutes.Subscribe(routes =>
        {
            _routes.Clear();
            _routes.AddRange(routes.Select(static route => new RouteViewModel(route)));
            GetArrivals!.Invoke(Stop);
        });

        // Arrivals
        GetArrivals = ReactiveCommand.CreateFromObservable<Stop, RouteArrival[]?>(client.GetArrivals);

        GetArrivals.Subscribe(arrivals =>
        {
            if (arrivals is null)
            {
                _routes.Edit(routes =>
                {
                    foreach (var route in routes)
                    {
                        route.Arrival = null;
                    }
                });
                _routesReSort.OnNext(Unit.Default);
                return;
            }

            var dict = arrivals.ToDictionary(static x => x.RouteId);
            _routes.Edit(routes =>
            {
                foreach (var route in routes)
                {
                    route.Arrival = dict.ContainsKey(route.Id) ? dict[route.Id] : null;
                }
            });
            _routesReSort.OnNext(Unit.Default);
        });

        this.WhenNavigatedTo((r, d) =>
        {
            // todo: Improve access to arguments.
            GetStop.Invoke(r[2]!).DisposeWith(d);

            // todo: In the future provide settings for the timer.
            // Get arrivals every X seconds.
            Observable
                .Interval(TimeSpan.FromSeconds(20))
                .ObserveOnTaskpool()
                .Select(_ => Stop)
                .WhereNotNull()
                .InvokeCommand(GetArrivals)
                .DisposeWith(d);
        });

        Observable.Merge(new[]
        {
            GetStop.ThrownExceptions,
            GetRoutes.ThrownExceptions,
            GetArrivals.ThrownExceptions,
        })
        .Subscribe(exs =>
        {
        });
    }

    public ReactiveCommand<string, Stop> GetStop { get; }

    public ReactiveCommand<Stop, Route[]> GetRoutes { get; }

    public ReactiveCommand<Stop, RouteArrival[]?> GetArrivals { get; }

    [Reactive]
    public Stop? Stop { get; private set; }

    [Reactive]
    public string? Title { get; private set; }

    public IObservable<IChangeSet<RouteViewModel>> Routes => _routes
        .Connect()
        .ObserveOnTaskpool()
        .Sort(SortExpressionComparer<RouteViewModel>.Ascending(static r => r.Order), resort: _routesReSort)
        .ObserveOnMain()
        .RefCount();

    public record RouteViewModel : Entity<RouteId>
    {
        private RouteArrival? _arrival;

        [SetsRequiredMembers]
        public RouteViewModel(Route route) : base(route.Id) => Route = route;

        public Route Route { get; }

        public RouteArrival? Arrival
        {
            get => _arrival;
            //set => this.RaiseAndSetIfChanged(ref _arrival, value);
            set => _arrival = value;
        }

        public int Order => Arrival?.Minutes[0] ?? 9999;
    }
}
