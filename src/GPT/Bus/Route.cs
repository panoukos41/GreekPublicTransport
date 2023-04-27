using StronglyTypedIds;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace GPT.Bus;

/// <summary>
/// Id for the <see cref="Route"/> entity.
/// Backed by the RouteCode value.
/// </summary>
[StronglyTypedId, DebuggerDisplay("{Value}")]
public partial struct RouteId : IId
{
    [IgnoreDataMember]
    public string Id => Value[2..];

    public static RouteId Parse(string routeId)
    {
        return new($"r:{routeId}");
    }
}

/// <summary>
/// A route represents a list of <see cref="Stop"/> objects,
/// and a description describing the start and the end of it.
/// </summary>
[DebuggerDisplay("Id: {Id}, Name: {Name}, Description: {Description}")]
public sealed record Route : Entity<RouteId>, IDescription
{
    /// <summary>
    /// The name of the line eg: 214
    /// </summary>
    public required string? Name { get; init; }

    /// <inheritdoc/>
    public required Description Description { get; init; }

    public LineId LineId { get; init; }

    public StopId[] StopIds { get; set; } = Array.Empty<StopId>();
}
