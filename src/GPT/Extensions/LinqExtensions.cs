namespace System.Collections.Generic;

public static class LinqExtensions
{
    public static bool AnyNull<T>(this IEnumerable<T> values)
        => values.Any(static x => x is null);

    public static bool AnyNotNull<T>(this IEnumerable<T> values)
        => values.Any(static x => x is { });

    public static IDictionary<TKey, TValue> ToDictionary<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> values)
        where TKey : notnull
        => values.ToDictionary(static p => p.Key, p => p.Value);
}
