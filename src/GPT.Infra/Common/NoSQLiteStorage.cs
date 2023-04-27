using NoSQLite;
using System.Reactive.Disposables;
using System.Reactive.Linq;

namespace GPT.Common;

public sealed class NoSQLiteStorage : IStorage
{
    private readonly NoSQLiteConnection db;
    private readonly NoSQLiteTable docs;

    public NoSQLiteStorage(NoSQLiteConnection db)
    {
        this.db = db;
        docs = db.GetTable();
    }

    /// <inheritdoc/>
    public IObservable<T?> Find<T>(string key)
    {
        return Observable.Create<T?>(o =>
        {
            var item = docs.Find<T>(key);
            o.OnNext(item);
            o.OnCompleted();
            return Disposable.Empty;
        });
    }

    /// <inheritdoc/>
    public IObservable<T[]> FindMany<T>(IEnumerable<string> keys)
    {
        return Observable.Create<T[]>(o =>
        {
            var items = docs.FindMany<T>(keys, false).ToArray();
            o.OnNext(items);
            o.OnCompleted();
            return Disposable.Empty;
        });
    }

    /// <inheritdoc/>
    public IObservable<IDictionary<string, T?>> FindPairs<T>(IEnumerable<string> keys)
    {
        return Observable.Create<IDictionary<string, T?>>(o =>
        {
            var pairs = docs.FindPairs<T>(keys);
            o.OnNext(pairs);
            o.OnCompleted();
            return Disposable.Empty;
        });
    }

    /// <inheritdoc/>
    public IObservable<None> Set<T>(string key, T item)
    {
        return Observable.Create<None>(o =>
        {
            docs.Insert(key, item);
            o.OnNext(None.Value);
            o.OnCompleted();
            return Disposable.Empty;
        });
    }

    /// <inheritdoc/>
    public IObservable<None> Set<T>(IDictionary<string, T> items)
    {
        return Observable.Create<None>(o =>
        {
            docs.InsertMany(items);
            o.OnNext(None.Value);
            o.OnCompleted();
            return Disposable.Empty;
        });
    }

    /// <inheritdoc/>
    public IObservable<None> Delete(string key)
    {
        return Observable.Create<None>(o =>
        {
            docs.Remove(key);
            o.OnNext(None.Value);
            o.OnCompleted();
            return Disposable.Empty;
        });
    }

    /// <inheritdoc/>
    public IObservable<None> Delete(IEnumerable<string> keys)
    {
        return Observable.Create<None>(o =>
        {
            docs.RemoveMany(keys);
            o.OnNext(None.Value);
            o.OnCompleted();
            return Disposable.Empty;
        });
    }

    /// <inheritdoc/>
    public IObservable<None> Clear()
    {
        return Observable.Create<None>(o =>
        {
            docs.Clear();
            o.OnNext(None.Value);
            o.OnCompleted();
            return Disposable.Empty;
        });
    }

    public IObservable<None> Checkpoint()
    {
        return Observable.Create<None>(o =>
        {
            db.Checkpoint();
            o.OnNext(None.Value);
            o.OnCompleted();
            return Disposable.Empty;
        });
    }

    /// <inheritdoc/>
    /// <remarks>
    /// This will dispose the database so only dispose
    /// if you know you won't be using this class again
    /// eg: when the application shutdowns
    /// </remarks>
    public void Dispose()
    {
        db.Dispose();
    }
}
