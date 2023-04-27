using StronglyTypedIds;
using System.Diagnostics;

namespace GPT.App;

[StronglyTypedId, DebuggerDisplay("Id: {Id}")]
public partial struct BookmarkId : IId { }

public sealed record Bookmark : Entity<BookmarkId>
{
    public required string PagePath { get; set; }

    public required string Title { get; set; }

    public required string Description { get; set; }
}
