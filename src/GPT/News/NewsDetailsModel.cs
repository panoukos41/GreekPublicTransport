using StronglyTypedIds;

namespace GPT.News;

[StronglyTypedId]
public partial struct NewsDetailsModelId : IId { }

public record NewsDetailsModel : Entity<NewsDetailsModelId>
{
    public NewsDetailsModel()// : base(NewsDetailsModelId.Empty)
    {

    }
}
