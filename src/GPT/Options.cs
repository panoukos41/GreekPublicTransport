using System.Text.Json;

namespace GPT;

public static class Options
{
    public static JsonSerializerOptions Json { get; } = new()
    {
        PropertyNamingPolicy = null,
        PropertyNameCaseInsensitive = true,
    };
}
