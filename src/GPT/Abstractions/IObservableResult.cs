namespace GPT.Abstractions;

public interface IObservableResult<T> : IObservable<Result<T>>
    where T : notnull
{
}
