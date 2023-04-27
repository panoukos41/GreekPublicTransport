using GPT;
using System.Reactive.Disposables;

namespace Spectre.Console;

public static class Mixins
{
    /// <summary>
    /// Unwraps the <see cref="ProgressContext"/> of a <see cref="Progress"/>.
    /// </summary>
    /// <param name="obj">The object to unwrap.</param>
    /// <returns>The context and an <see cref="IDisposable"/> to signal the progress window has finished.</returns>
    /// <remarks>You must always call Dispose on the Disposable that has been returned.</remarks>
    public static (ProgressContext, IDisposable) Start(this Progress obj)
    {
        ProgressContext context = null!;
        IDisposable disposable = null!;

        var observable = Observable.Create<ProgressContext>(obs =>
        {
            TaskCompletionSource<None> tsk = new();
            obj.StartAsync(async ctx =>
            {
                obs.OnNext(ctx);
                await tsk.Task;
                obs.OnCompleted();
            });
            return Disposable.Create(() => tsk.SetResult(None.Value));
        });

        disposable = observable.Subscribe(ctx => context = ctx);

        return (context, disposable);
    }

    /// <summary>
    /// Unwraps the <see cref="StatusContext"/> of a <see cref="Status"/>.
    /// </summary>
    /// <param name="obj">The object to unwrap.</param>
    /// <param name="status">The status to display.</param>
    /// <returns>The context and an <see cref="IDisposable"/> to signal the status window has finished.</returns>
    /// <remarks>You must always call Dispose on the Disposable that has been returned.</remarks>
    public static (StatusContext, IDisposable) Start(this Status obj, string status)
    {
        StatusContext context = null!;
        IDisposable disposable = null!;

        var observable = Observable.Create<StatusContext>(obs =>
        {
            TaskCompletionSource<None> tsk = new();
            obj.StartAsync(status, async ctx =>
            {
                obs.OnNext(ctx);
                await tsk.Task;
                obs.OnCompleted();
            });
            return Disposable.Create(() =>
                tsk.SetResult(None.Value));
        });

        disposable = observable.Subscribe(ctx => context = ctx);

        return (context, disposable);
    }
}
