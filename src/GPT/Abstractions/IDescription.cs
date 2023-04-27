using GPT.App;

namespace GPT.Abstractions;

public interface IDescription
{
    /// <summary></summary>
    Description Description { get; init; }
}

/// <summary>
/// Class that stores descriptions for certain languages.
/// </summary>
public class Description
{
    /// <summary></summary>
    /// <remarks>If no value is provided 'Empty Description' is returned.</remarks>
    public string? English { get; init; }

    /// <summary></summary>
    /// <remarks>If no value is provided 'Άδεια περιγραφή' is returned.</remarks>
    public string? Greek { get; init; }

    public override string ToString() => Language.Current.Name switch
    {
        nameof(Language.Greek) => Greek ?? English ?? emptyDescription,
        _ => English ?? emptyDescription
    };

    private const string emptyDescription = "Empty Description!";

    public static implicit operator string(Description d) => d.ToString();
}
