namespace GPT;

public static class Services
{
    private static IServiceProvider? _services;

    public static IDisposable Initialize(IServiceProvider services)
    {
        _services ??= services;
        return Disposable.Create(Dispose);
    }

    public static T Resolve<T>()
    {
        return _services is { }
            ? _services.GetService(typeof(T)) is { } s
                ? (T)s
                : throw new NotImplementedException($"Could not locate service {typeof(T)}")
            : throw new InvalidOperationException("Call initialize before trying to resolve services");
    }

    public static void Dispose()
    {
        if (_services is IDisposable disposable)
        {
            disposable.Dispose();
        }
    }
}
