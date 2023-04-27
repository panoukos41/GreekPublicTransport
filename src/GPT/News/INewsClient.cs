namespace GPT.News;

public interface INewsClient
{
    public IObservable<NewsModel> GetNews(int limit = 20);

    public IObservable<NewsDetailsModel> GetNewsDetails(NewsModel news);
}
