using System.Reactive.Linq;

namespace GPT.Abstractions;

public interface IStorage : IDisposable
{
    IObservable<T?> Find<T>(string key);

    IObservable<T[]> FindMany<T>(IEnumerable<string> keys);

    IObservable<IDictionary<string, T?>> FindPairs<T>(IEnumerable<string> keys);

    IObservable<None> Set<T>(string key, T item);

    IObservable<None> Set<T>(IDictionary<string, T> items);

    IObservable<None> Delete(string key);

    IObservable<None> Delete(IEnumerable<string> keys);

    IObservable<None> Clear();

    IObservable<None> Checkpoint();
}

public static class IStorageMixins
{
    public static IObservable<None> Set<T>(this IStorage storage, T[] items)
        where T : IEntity
    {
        var dict = new Dictionary<string, T>(items.Length);
        foreach (var item in items) dict[item.Id.Value] = item;
        return storage.Set(dict);
    }

    public static void Store<T>(this T entity, IStorage storage)
        where T : IEntity
    {
        storage.Set(entity.Id.Value, entity).Subscribe();
    }

    public static IObservable<T> Store<T>(this IObservable<T> source, IStorage storage)
        where T : IEntity
    {
        return source.Do(x => storage.Set(x.Id.Value, x).Subscribe());
    }

    public static IObservable<T[]> Store<T>(this IObservable<T[]> source, IStorage storage)
        where T : IEntity
    {
        return source.Do(x => storage.Set(x).Subscribe());
    }

    public static IObservable<IDictionary<string, T>> Store<T>(this IObservable<IDictionary<string, T>> source, IStorage storage)
    {
        return source.Do(x => storage.Set(x).Subscribe());
    }

    /// <summary>
    /// Will try to find the entity of the given <paramref name="id"/>
    /// and if it can't it will retrieve it using the <paramref name="get"/>
    /// function and then store it.
    /// </summary>
    /// <param name="id">The id to search for.</param>
    /// <param name="get">The function that returns the entity if not found.</param>
    /// <returns>The entity for the given key.</returns>
    public static IObservable<TEntity> FindOrGet<TEntity>(
        this IStorage storage,
        string id,
        Func<IObservable<TEntity>> get)
        where TEntity : notnull, IEntity
    {
        return storage
            .Find<TEntity>(id)
            .SelectMany(entity => entity switch
            {
                null => get().Store(storage),
                _ => Observable.Return(entity),
            });
    }

    /// <summary>
    /// Will try to find the entities of the given <paramref name="ids"/>
    /// and if all or any can't be found it will retrieve them by using the <paramref name="get"/>
    /// function. This will only set the missing keys even if the function returns all.
    /// </summary>
    /// <param name="ids">The ids to search for.</param>
    /// <param name="get">The function that returns the entities if not found.</param>
    /// <returns>The entities in key - value pairs.</returns>
    public static IObservable<TEntity[]> FindOrGet<TId, TEntity>(
        this IStorage storage,
        TId[] ids,
        Func<IObservable<TEntity[]>> get)
        where TId : struct, IId
        where TEntity : notnull, IEntity
    {
        return storage.FindOrGet(ids.Select(static id => id.Value).ToArray(), get);
    }

    /// <summary>
    /// Will try to find the entities of the given <paramref name="ids"/>
    /// and if all or any can't be found it will retrieve them by using the <paramref name="get"/>
    /// function. This will only set the missing keys even if the function returns all.
    /// </summary>
    /// <param name="ids">The ids to search for.</param>
    /// <param name="get">The function that returns the entities if not found.</param>
    /// <returns>The entities in key - value pairs.</returns>
    public static IObservable<TEntity[]> FindOrGet<TEntity>(
        this IStorage storage,
        string[] ids,
        Func<IObservable<TEntity[]>> get)
        where TEntity : notnull, IEntity
    {
        static IObservable<IDictionary<string, TEntity>> Empty()
            => Observable.Return(new Dictionary<string, TEntity>());

        return storage
            .FindPairs<TEntity>(ids)
            .SelectMany(pairs => pairs switch
            {
                // Nothing exists we need to get everything
                { Count: 0 } => get()
                    .Do(got => ids = got.Select(x => x.Id.Value).ToArray())
                    .ToDictionary()
                    .CombineLatest(Empty(), (got, pairs) =>
                    {
                        pairs = storage.FindPairs<TEntity>(ids).FirstAsync().Wait();
                        // Find the stored values and replace them in the result.
                        foreach (var (key, value) in pairs.Where(pair => pair.Value is { }))
                        {
                            got[key] = value!;
                        }
                        return got;
                    })
                    .Store(storage),

                // We have some values but not all so we have to
                // retrieve them and set the ones we miss.
                { } v when v.AnyNull() =>
                    get()
                    .ToDictionary()
                    .CombineLatest(Observable.Return(pairs), static (got, pairs) =>
                    {
                        // Find the missing values and put them in the result.
                        foreach (var (key, _) in pairs.Where(pair => pair.Value is null))
                        {
                            pairs[key] = got[key];
                        }
                        return pairs;
                    })
                    .Store(storage),

                // We found everything so we just return the values.
                _ => Observable
                    .Return(pairs)
            })
            .Select(pairs =>
            {
                var count = ids.Length;
                var items = new TEntity[count];
                for (int i = 0; i < count; i++)
                {
                    var key = ids[i];
                    items[i] = pairs[key];
                }
                return items;
            });
    }
}
