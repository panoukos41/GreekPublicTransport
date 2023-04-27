using GPT;
using System.Reactive;
using System.Reactive.Linq;

namespace ReactiveUI;

public static class ReactiveCommandExtensions
{
    // todo: Summary

    public static IDisposable Invoke<TResult>(this ReactiveCommand<None, TResult> command)
        => Observable.Return(None.Value).InvokeCommand(command);

    public static IDisposable Invoke<TResult>(this ReactiveCommand<Unit, TResult> command)
        => Observable.Return(Unit.Default).InvokeCommand(command);

    public static IDisposable Invoke<TInput, TResult>(this ReactiveCommand<TInput, TResult> command, TInput input)
        => Observable.Return(input).InvokeCommand(command);
}
