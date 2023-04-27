using FluentValidation;
using System.Diagnostics.CodeAnalysis;

namespace GPT.Common;

public sealed record Location : IValid
{
    public required double Latitude { get; init; }

    public required double Longitude { get; init; }

    public Location()
    {
    }

    [SetsRequiredMembers]
    public Location(double latitude, double longitude)
    {
        Latitude = latitude;
        Longitude = longitude;
    }

    public static Location Empty { get; } = new(0, 0);

    public static IValidator Validator { get; } = new InlineValidator<Location>
    {
        static v => v.RuleFor(x => x.Latitude).InclusiveBetween(-90,90),
        static v => v.RuleFor(x => x.Longitude).InclusiveBetween(-180,180),
    };
}
