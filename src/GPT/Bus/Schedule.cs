using StronglyTypedIds;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace GPT.Bus;

/// <summary>
/// Id for the <see cref="Schedule"/> entity.
/// Backed by the <see cref="Models.LineId"/>, <see cref="Models.MasterLineId"/> and SdcCode values.
/// </summary>
/// <remarks>
/// For the desired effect you should always use the 
/// <see cref="ScheduleId(Models.LineId, Models.MasterLineId, string)"/> constructor.
/// </remarks>
[StronglyTypedId, DebuggerDisplay("{Value}")]
public partial struct ScheduleId : IId
{
    [IgnoreDataMember]
    public string Name => GetValue(Value, 1);

    [IgnoreDataMember]
    public LineId LineId => LineId.Parse(GetValue(Value, 2));

    [IgnoreDataMember]
    public string SdcCode => GetValue(Value, 3);

    public ScheduleId(LineId lineId, string name, string sdcCode)
        : this($"{name}.{lineId}.{sdcCode}")
    {
    }

    public static ScheduleId Parse(LineId lineId, string name, string sdcCode)
    {
        return new($"{name}.{lineId}.{sdcCode}");
    }

    private static string GetValue(string value, int position)
    {
        var first = value.IndexOf('.');
        var second = value.IndexOf('.', first + 1);

        return position switch
        {
            1 => value[..first],
            2 => value[(first + 1)..second],
            3 => value[(second + 1)..],
            _ => throw new ArgumentException("Position must be from 1 through 3", nameof(position))
        };
    }
}

[DebuggerDisplay("Id: {Id}, Description: {Description}")]
public sealed record Schedule : Entity<ScheduleId>, IDescription
{
    /// <inheritdoc/>
    public required Description Description { get; init; }

    public TimeOnly[] FromStartingPoint { get; init; } = Array.Empty<TimeOnly>();

    public TimeOnly[] FromTerminalPoint { get; init; } = Array.Empty<TimeOnly>();
}
