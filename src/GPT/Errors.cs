namespace GPT;

public static class Errors
{
    public static Er NoInternet { get; } = new()
    {
        Error = "No Internet",
        Reason = "No internet connnection available"
    };
}
