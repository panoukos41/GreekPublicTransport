using Flurl;
using Flurl.Http;
using System.Reactive.Linq;

namespace GPT.Bus;

public sealed class OasaBusCliecnt : IBusClient
{
    private readonly IFlurlClient _client;

    public OasaBusCliecnt(IFlurlClient client) => _client = client;

    private IFlurlRequest Request => _client.Request("http://telematics.oasa.gr/api/");

    // todo: Filter all lines based on name and keep the one with the smaller id
    // or change line object to keep groups of diferrent lines maybe.

    /// <inheritdoc/>
    public IObservable<Line[]> GetLines() => GetLineDtos()
        .Select(static dtos =>
        {
            // todo: If dtos is null throw a predefined error with custom message.

            var lines = new Line[dtos.Length];
            for (int i = 0; i < dtos.Length; i++)
            {
                var dto = dtos[i];
                lines[i] = new Line
                {
                    Id = new(dto.LineCode),
                    Name = dto.LineId,
                    Description = new()
                    {
                        English = dto.DescriptionEn,
                        Greek = dto.DescriptionGr
                    }
                };
            }
            return lines;
        });

    /// <inheritdoc/>
    public IObservable<Line> GetLine(LineId lineId) => GetLineDtos()
        .SelectMany(static dtos => dtos)
        .FirstAsync(dto => dto.LineCode == lineId.Value)
        .Select(static dto => new Line
        {
            Id = new(dto.LineCode),
            Name = dto.LineId,
            Description = new()
            {
                English = dto.DescriptionEn,
                Greek = dto.DescriptionGr
            }
        });

    private IObservable<LineDto[]> GetLineDtos() =>
        Request
        .SetQueryParam("act", "webGetLines")
        .GetJsonObservable<LineDto[]>();

    /// <inheritdoc/>
    public IObservable<Route[]> GetRoutesForLine(LineId lineId, string lineName) =>
        Request
        .SetQueryParam("act", "getRoutesForLine")
        .SetQueryParam("p1", lineId)
        .GetJsonObservable<RouteDto[]>()
        .SelectMany(static dtos => dtos)
        .Select(dto => new Route
        {
            Id = new(dto.RouteCode),
            LineId = lineId,
            Name = lineName,
            Description = new()
            {
                English = dto.DescriptionEn,
                Greek = dto.DescriptionGr
            }
        })
        .ToArray();

    /// <inheritdoc/>
    public IObservable<Stop[]> GetStopsForRoute(RouteId routeId) =>
        Request
        .SetQueryParam("act", "webGetStops")
        .SetQueryParam("p1", routeId)
        .GetJsonObservable<RouteDto.Stop[]>()
        .SelectMany(static dtos => dtos)
        .Select(dto => new Stop
        {
            Id = new(dto.StopCode),
            Description = new()
            {
                English = dto.DescriptionEn,
                Greek = dto.DescriptionGr
            },
            MapCoordinates = new()
            {
                Latitude = dto.Latitude,
                Longitude = dto.Longitude
            }
            //IsAmea = stop.IsAmea todo: Fix IsAmea value, search if it holds true.
        })
        .ToArray();

    // todo: Handle null cases somehow in database!



    /// <inheritdoc/>
    public IObservable<Schedule[]> GetSchedules(LineId lineId, string name) =>
        Request
        .SetQueryParam("act", "getScheduleDaysMasterline")
        .SetQueryParam("p1", lineId)
        .GetJsonObservable<ScheduleDto[]>()
        .SelectMany(dtos => dtos is null
            ? Observable.Return<Schedule[]?>(null)
            : dtos.ToObservable()
            .SelectMany(dto =>
                Request
                .SetQueryParam("act", "getSchedLines")
                .SetQueryParam("p1", name)
                .SetQueryParam("p2", dto.SdcCode)
                .SetQueryParam("p3", lineId)
                .GetJsonObservable<ScheduleTimetableDto>()
                .Select(timetable => new
                {
                    dto.SdcCode,
                    timetable?.Go,
                    timetable?.Come
                }))
            .ToArray()
            .Select(timetables => timetables.ToDictionary(static x => x.SdcCode))
            .Select(tables =>
            {
                var schedules = new Schedule[dtos.Length];
                for (int i = 0; i < dtos.Length; i++)
                {
                    var dto = dtos[i];
                    schedules[i] = new Schedule
                    {
                        Id = new(lineId, name, dto.SdcCode),
                        Description = new()
                        {
                            English = dto.SdcDescrEng,
                            Greek = dto.SdcDescrGr
                        },
                        FromStartingPoint = tables[dto.SdcCode]?.Go ?? Array.Empty<TimeOnly>(),
                        FromTerminalPoint = tables[dto.SdcCode]?.Come ?? Array.Empty<TimeOnly>()
                    };
                }
                return schedules;
            }));

    ///// <inheritdoc/>
    //public IObservable<ScheduleTimeTable> GetScheduleTimetable(ScheduleId schedulelId) =>
    //    Request
    //    .SetQueryParam("act", "getSchedLines")
    //    .SetQueryParam("p1", schedulelId.Name)
    //    .SetQueryParam("p2", schedulelId.SdcCode)
    //    .SetQueryParam("p3", schedulelId.LineId)
    //    .GetJsonObservable<ScheduleTimetableDto>()
    //    .Select(schedules => new ScheduleTimeTable
    //    {
    //        Id = new(schedulelId),
    //        FromStartingPoint = schedules.Go,
    //        FromTerminalPoint = schedules.Come
    //    });

    /// <inheritdoc/>
    public IObservable<Stop> GetStop(StopId stopId) =>
        Request
        .SetQueryParam("act", "getStopNameAndXY")
        .SetQueryParam("p1", stopId)
        .GetJsonObservable<StopDto[]>()
        .SelectMany(static dtos => dtos)
        .Select(dto => new Stop
        {
            Id = new(dto.StopCode),
            Description = new()
            {
                English = dto.DescriptionEn,
                Greek = dto.DescriptionGr
            },
            MapCoordinates = new()
            {
                Latitude = dto.Latitude,
                Longitude = dto.Longitude
            }
            //IsAmea = todo: check for IsAmea.
        });

    /// <inheritdoc/>
    public IObservable<Route[]> GetRoutesForStop(StopId stopId) =>
        Request
        .SetQueryParam("act", "webRoutesForStop")
        .SetQueryParam("p1", stopId)
        .GetJsonObservable<StopDto.Route[]>()
        .SelectMany(static dtos => dtos)
        .Where(static route => route.Hidden == "0")
        .Select(dto => new Route
        {
            Id = new(dto.RouteCode),
            Name = dto.LineId,
            LineId = new(dto.LineCode),
            Description = new()
            {
                English = dto.DescriptionEn,
                Greek = dto.DescriptionGr
            }
        })
        .ToArray();

    /// <inheritdoc/>
    public IObservable<RouteArrival[]> GetArrivals(StopId stopId) =>
        Request
        .SetQueryParam("act", "getStopArrivals")
        .SetQueryParam("p1", stopId)
        .GetJsonObservable<ArrivalDto[]>()
        .SelectMany(static dto => dto
            .GroupBy(static x => x.RouteCode)
            .Select(group => new RouteArrival
            {
                RouteId = new(group.Key),
                Minutes = group.Select(x => x.Time).ToArray()
            }))
        .ToArray();
}
