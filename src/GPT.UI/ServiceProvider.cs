using Flurl.Http;
using GPT.App;
using GPT.Bus;
using GPT.Common;
using Jab;
using NoSQLite;
using P41.Navigation;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace GPT;

// Main Services
[Singleton<IBusClient, OasaBusCliecnt>]
[Singleton<IStorage, NoSQLiteStorage>]
[Singleton<INavigationHost>(Factory = nameof(GetNavigationHost))]

// Other Services
[Singleton<IFlurlClient, FlurlClient>]
[Singleton<NoSQLiteConnection>(Factory = nameof(GetNoSQLiteConnection))]

// Options
[Singleton<JsonSerializerOptions>(Factory = nameof(GetJsonSerializerOptions))]

// ViewModels
[Transient<BookmarkViewModel>]
[Transient<LineCollectionViewModel>]
[Transient<LineDetailsViewModel>]
[Transient<StopDetailsViewModel>]

[ServiceProvider]
public abstract partial class ServiceProvider
{
    private static JsonSerializerOptions GetJsonSerializerOptions()
    {
        var options = new JsonSerializerOptions
        {
            WriteIndented = false,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.GreekandCoptic),
        };
        return options;
    }

    private NoSQLiteConnection GetNoSQLiteConnection(JsonSerializerOptions options)
    {
        var path = Path.Combine(DatabaseDirectory, "gpt.sqlite3");

        return new NoSQLiteConnection(path, options);
    }

    /// <summary>
    /// The full path to the database directory.
    /// </summary>
    protected abstract string DatabaseDirectory { get; }

    /// <summary>
    /// The platform specific navigation host.
    /// </summary>
    protected abstract INavigationHost GetNavigationHost();
}
