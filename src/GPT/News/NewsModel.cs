using StronglyTypedIds;

namespace GPT.News;

[StronglyTypedId]
public partial struct NewsModelId : IId { }

public record NewsModel : Entity<NewsModelId>
{

}
