using GPT;

namespace System.Reactive.Linq;

/// <summary/>
public static class ObservableExtensions
{
    /// <summary>
    /// Ignore any result of the observable and just return <see cref="Unit"/>.
    /// </summary>
    public static IObservable<Unit> ToUnit<T>(this IObservable<T> source)
    {
        return source.Select(static _ => Unit.Default);
    }

    /// <summary>
    /// Ignore any result of the observable and just return <see cref="Unit"/>.
    /// </summary>
    public static IObservable<None> ToNone<T>(this IObservable<T> source)
    {
        return source.Select(static _ => None.Value);
    }

    /// <summary>
    /// True when the emmited value is null.
    /// </summary>
    public static IObservable<bool> IsNull<T>(this IObservable<T> source)
    {
        return source.Select(static x => x is null);
    }

    /// <summary>
    /// True when the emmited value is not null.
    /// </summary>
    public static IObservable<bool> IsNotNull<T>(this IObservable<T> source)
    {
        return source.Select(static x => x is { });
    }

    /// <summary>
    /// Only allow null values to pass.
    /// </summary>
    public static IObservable<T?> WhereNull<T>(this IObservable<T> source)
    {
        return source.Where(static x => x is null);
    }

    /// <summary>
    /// If the condition is true execute the action using <b>Do</b> otherwise return the source.
    /// </summary>
    public static IObservable<T> WhenCondition<T>(this IObservable<T> source, bool condition, Action<T> action)
    {
        return condition ? source.Do(action) : source;
    }

    /// <summary>
    /// Turn an <see cref="IEnumerable{T}"/> of <see cref="IEntity"/> 
    /// to an <see cref="IDictionary{TKey, TValue}"/>
    /// </summary>
    public static IObservable<IDictionary<string, TEntity>> ToDictionary<TEntity>(this IObservable<IEnumerable<TEntity>> source)
        where TEntity : IEntity
    {
        return source.Select(static items =>
        {
            _ = items.TryGetNonEnumeratedCount(out var count);
            var dictionary = new Dictionary<string, TEntity>(count);

            foreach (var item in items)
            {
                if (dictionary.ContainsKey(item.Id.Value)) continue;

                dictionary.Add(item.Id.Value, item);
            }
            return dictionary;
        });
    }
}
