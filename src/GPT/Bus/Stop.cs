using StronglyTypedIds;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace GPT.Bus;

/// <summary>
/// Id for the <see cref="Stop"/> entity.
/// Backed by the StopCode value.
/// </summary>
[StronglyTypedId, DebuggerDisplay("{Value}")]
public partial struct StopId : IId
{
    [IgnoreDataMember]
    public string Id => Value[2..];

    public static StopId Parse(string stopId)
    {
        return new($"s:{stopId}");
    }
}

/// <summary>
/// Information about a bus stop.
/// </summary>
[DebuggerDisplay("Id: {Id}, Description: {Description}")]
public record Stop : Entity<StopId>, IDescription
{
    /// <inheritdoc/>
    public required Description Description { get; init; }

    /// <inheritdoc/>
    public MapCoordinates? MapCoordinates { get; init; }

    public RouteId[] RouteIds { get; set; } = Array.Empty<RouteId>();

    public bool IsAmea { get; init; }
}

/// <summary>
/// Arrival information for a route at a bus stop.
/// </summary>
/// <remarks>
/// <see cref="ToString"/> returns a string for display,
/// if this is null you can use the static <see cref="String"/>
/// </remarks>
[DebuggerDisplay("Id: {RouteId}, Display: {Display}")]
public sealed record RouteArrival
{
    public required RouteId RouteId { get; init; }

    public required int[] Minutes { get; init; }

    public string Display => ToString();

    public override string ToString()
        => Minutes.Length == 0 ? String : string.Join(" & ", Minutes.OrderBy(x => x));

    public const string String = "...";
}

/// <summary>
/// Class that stores map coordinates.
/// </summary>
public record MapCoordinates
{
    /// <summary></summary>
    public required string Longitude { get; init; }

    /// <summary></summary>
    public required string Latitude { get; init; }
}