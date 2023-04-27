using StronglyTypedIds;

[assembly: StronglyTypedIdDefaults(
    backingType: StronglyTypedIdBackingType.String,
    converters: StronglyTypedIdConverter.SystemTextJson,
    implementations: StronglyTypedIdImplementations.Default)]
