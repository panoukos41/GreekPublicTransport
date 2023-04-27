using OneOf;

namespace System.Reactive.Linq;

public static class OneOfObservableExtensions
{
    /// <summary>
    /// Subscribe and execute the <see cref="OneOf{T0, T1}.Switch(Action{T0}, Action{T1})"/> method.
    /// </summary>
    public static IDisposable Switch<T0, T1>(this IObservable<OneOf<T0, T1>> observable, Action<T0> t0, Action<T1> t1)
        => observable.Subscribe(oneof => oneof.Switch(t0, t1));

    /// <summary>
    /// Subscribe and execute the <see cref="OneOf{T0, T1}.Match{TResult}(Func{T0, TResult}, Func{T1, TResult})"/> method.
    /// </summary>
    public static IDisposable Match<T0, T1, TResult>(this IObservable<OneOf<T0, T1>> observable, Func<T0, TResult> t0, Func<T1, TResult> t1)
        => observable.Subscribe(oneof => oneof.Match(t0, t1));

    /// <summary>
    /// Allows items whose <see cref="OneOf{T0, T1}.Value"/> is not null.
    /// </summary>
    public static IObservable<OneOf<T0, T1>> WhereNotNull<T0, T1>(this IObservable<OneOf<T0, T1>> observable)
        => observable.Where(static oneof => oneof.Value is { });

    /// <summary>
    /// Allows items whose <see cref="OneOf{T0, T1}.Value"/> is <typeparamref name="T0"/>.
    /// </summary>
    public static IObservable<OneOf<T0, T1>> WhereT0<T0, T1>(this IObservable<OneOf<T0, T1>> observable)
        => observable.Where(static oneof => oneof.Value is T0);

    /// <summary>
    /// Allows items whose <see cref="OneOf{T0, T1}.Value"/> is <typeparamref name="T1"/>.
    /// </summary>
    public static IObservable<OneOf<T0, T1>> WhereT1<T0, T1>(this IObservable<OneOf<T0, T1>> observable)
        => observable.Where(static oneof => oneof.Value is T1);

    /// <summary>
    /// Will select <typeparamref name="T0"/> if <see cref="OneOf{T0, T1}.IsT0"/> is true.
    /// </summary>
    public static IObservable<T0> SelectT0<T0, T1>(this IObservable<OneOf<T0, T1>> observable)
        => observable.WhereT0().Select(static oneof => oneof.AsT0);

    /// <summary>
    /// Will select <typeparamref name="T1"/> if <see cref="OneOf{T0, T1}.IsT1"/> is true.
    /// </summary>
    public static IObservable<T1> SelectT1<T0, T1>(this IObservable<OneOf<T0, T1>> observable)
        => observable.WhereT1().Select(static oneof => oneof.AsT1);
}
