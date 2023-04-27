namespace System.Reactive.Linq;

using System;

public static class LinqReactiveExtensions
{
    public static IObservable<T> ObserveOnMain<T>(this IObservable<T> observable)
        => observable.ObserveOn(RxApp.MainThreadScheduler);

    public static IObservable<T> ObserveOnTaskpool<T>(this IObservable<T> observable)
        => observable.ObserveOn(RxApp.TaskpoolScheduler);

    public static IDisposable SubscribeMain<T>(this IObservable<T> observable, Action<T> onNext)
    => observable.ObserveOnMain().Subscribe(onNext);
}
