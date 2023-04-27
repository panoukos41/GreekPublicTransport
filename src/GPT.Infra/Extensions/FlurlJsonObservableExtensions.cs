using GPT;
using System.Reactive.Linq;
using System.Text.Json;

namespace Flurl.Http;

public static class FlurlJsonObservableExtensions
{
    // todo: Check and improve for error handling.

    /// <summary>
    /// Deserialize the content and emit it.
    /// </summary>
    /// <typeparam name="T">The type to deserialize to.</typeparam>
    public static IObservable<T> GetJsonObservable<T>(this IFlurlRequest request) =>
        Observable
        .FromAsync(token => request.GetStreamAsync(token))
        .Where(static stream => stream.Length != stream.Position)
        .Select(static stream => JsonSerializer.Deserialize<T>(stream, Options.Json));


    /// <summary>
    /// Deserialize the content and emit it.
    /// </summary>
    /// <typeparam name="T">The type to deserialize to.</typeparam>
    public static IObservable<T> GetJsonObservable<T>(this IFlurlRequest request, JsonSerializerOptions options) =>
        Observable
        .FromAsync(token => request.GetStreamAsync(token))
        .Where(static stream => stream.Length != stream.Position)
        .Select(stream => JsonSerializer.Deserialize<T>(stream, options));
}
