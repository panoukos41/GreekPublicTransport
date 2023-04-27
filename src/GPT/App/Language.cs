using Ardalis.SmartEnum;
using System.Globalization;

namespace GPT.App;

public class Language : SmartEnum<Language, string>
{
    public static readonly Language English = new("English", "en-US");

    public static readonly Language Greek = new("Ελληνικά", "el-GR");

    private Language(string name, string value) : base(name, value)
    {
    }

    static Language()
    {
        Current = TryFromValue(CultureInfo.CurrentUICulture.Name, out var lang) ? lang : English;
    }

    private static Language _current = null!;
    //private static readonly Subject<Language> _subject = new();

    public static Language Current
    {
        get => _current;
        set
        {
            _current = value;
            var cult = new CultureInfo(value.Value);
            CultureInfo.DefaultThreadCurrentCulture = cult;
            CultureInfo.DefaultThreadCurrentUICulture = cult;
            //_subject.OnNext(_current);
        }
    }

    ///// <summary>
    ///// Event for when the current language is changed.
    ///// </summary>
    //public static IObservable<Language> Changed => _subject;
}
