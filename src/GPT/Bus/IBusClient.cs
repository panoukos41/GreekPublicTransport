namespace GPT.Bus;

/// <summary>
/// Client to retrieve Lines/Routes/Schedules/Stops and Arrivals from the network.
/// </summary>
public interface IBusClient
{
    /// <summary>
    /// Get all available bus lines.
    /// </summary>
    IObservable<Line[]> GetLines();

    /// <summary>
    /// Get a specific bus line.
    /// </summary>
    IObservable<Line> GetLine(LineId lineId);

    /// <summary>
    /// Get all available routes for a <see cref="LineId"/>.
    /// </summary>
    IObservable<Route[]> GetRoutesForLine(LineId lineId, string lineName);

    /// <summary>
    /// Get all available stops for a <see cref="RouteId"/>.
    /// </summary>
    IObservable<Stop[]> GetStopsForRoute(RouteId routeId);

    /// <summary>
    /// Get all available schedules for a <see cref="LineId"/> and a name combination.
    /// </summary>
    /// <remarks>
    /// This can sometimes return null if no schedules are found.
    /// </remarks>
    IObservable<Schedule[]> GetSchedules(LineId lineId, string name);

    /// <summary>
    /// Get a bus stop for a <see cref="StopId"/>.
    /// </summary>
    IObservable<Stop> GetStop(StopId id);

    /// <summary>
    /// Get all available routes for a <see cref="StopId"/>.
    /// </summary>
    IObservable<Route[]> GetRoutesForStop(StopId stopId);

    /// <summary>
    /// Get bus/route arrival times for <see cref="StopId"/>.
    /// </summary>
    IObservable<RouteArrival[]> GetArrivals(StopId id);
}

public static class IBusClientMixins
{
    public static IObservable<Route[]> GetRoutesForLine(this IBusClient service, Line line) =>
        service.GetRoutesForLine(line.Id, line.Name);

    public static IObservable<Stop[]> GetStopsForRoute(this IBusClient service, Route route) =>
        service.GetStopsForRoute(route.Id);

    public static IObservable<Schedule[]> GetSchedules(this IBusClient service, Line line) =>
        service.GetSchedules(line.Id, line.Name);

    public static IObservable<Route[]> GetRoutesForStop(this IBusClient service, Stop stop) =>
        service.GetRoutesForStop(stop.Id);

    public static IObservable<RouteArrival[]> GetArrivals(this IBusClient service, Stop stop) =>
        service.GetArrivals(stop.Id);
}
