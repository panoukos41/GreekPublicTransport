using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;

namespace GPT.Abstractions;

public interface IId
{
    public string Value { get; }
}

/// <summary>
/// Defines an entity object.
/// </summary>
public interface IEntity
{
    /// <summary>
    /// The entity's unique Id.
    /// </summary>
    IId Id { get; }

    /// <summary>
    /// The exact moment this object was created.
    /// </summary>
    DateOnly Created { get; init; }
}

/// <summary>
/// Base entity type to define entities. <br/>
/// Implements: <see cref="IEntity"/> <br/>
/// </summary>
public abstract record Entity<TId> : IEntity
    where TId : struct, IId
{
    /// <summary>
    /// Initialize new entity with default Id and Created/Updated equal to UtcNow.
    /// </summary>
    protected Entity()
    {
    }

    /// <summary>
    /// Initialize a new Entity with an Id and Created/Updated equal to UtcNow.
    /// </summary>
    /// <param name="id"></param>
    [SetsRequiredMembers]
    protected Entity(TId id)
    {
        Id = id;
        Created = DateOnly.FromDateTime(DateTime.Now);
    }

    /// <inheritdoc/>
    [JsonPropertyOrder(-1)]
    public required TId Id { get; init; }

    /// <inheritdoc/>
    [JsonPropertyOrder(100)]
    public DateOnly Created { get; init; }

    [JsonIgnore]
    IId IEntity.Id => Id!;
}
