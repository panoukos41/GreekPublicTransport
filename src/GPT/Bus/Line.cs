using StronglyTypedIds;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace GPT.Bus;

/// <summary>
/// Id for the <see cref="Line"/> entity.
/// Backed by the LineCode value.
/// </summary>
[StronglyTypedId, DebuggerDisplay("{Value}")]
public partial struct LineId : IId
{
    [IgnoreDataMember]
    public string Id => Value[2..];

    public static LineId Parse(string lineId)
    {
        return new($"l:{lineId}");
    }
}

/// <summary>
/// Bus line object containing info for a line.
/// </summary>
[DebuggerDisplay("Id: {Id}, Name: {Name}")]
public sealed record Line : Entity<LineId>, IDescription
{
    /// <summary>
    /// The name of the line eg: 214
    /// </summary>
    public required string Name { get; init; }

    /// <inheritdoc/>
    public required Description Description { get; init; }

    public RouteId[] RouteIds { get; set; } = Array.Empty<RouteId>();

    public ScheduleId[] SchedulelIds { get; set; } = Array.Empty<ScheduleId>();
}
