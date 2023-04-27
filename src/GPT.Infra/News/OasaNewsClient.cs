namespace GPT.News;

public class OasaNewsClient : INewsClient
{
    public IObservable<NewsModel> GetNews(int limit = 20)
    {
        throw new NotImplementedException();
    }

    public IObservable<NewsDetailsModel> GetNewsDetails(NewsModel news)
    {
        throw new NotImplementedException();
    }
}
