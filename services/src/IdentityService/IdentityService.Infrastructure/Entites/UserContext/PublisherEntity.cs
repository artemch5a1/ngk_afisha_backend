using System.ComponentModel.DataAnnotations.Schema;
using IdentityService.Domain.Abstractions.Infrastructure.Entity;
using IdentityService.Domain.Models.UserContext;

namespace IdentityService.Infrastructure.Entites.UserContext;

public class PublisherEntity : IEntity<PublisherEntity, Publisher>
{
    [Column("publisher_id")]
    public Guid PublisherId { get; set; }

    [Column("post_id")]
    public int PostId { get; set;  }

    [ForeignKey(nameof(PostId))]
    public PostEntity Post { get; set; } = null!;

    [ForeignKey(nameof(PublisherId))]
    public UserEntity User { get; set; } = null!;

    internal PublisherEntity()
    {
        
    }

    private PublisherEntity(Publisher publisher)
    {
        PublisherId = publisher.PublisherId;

        PostId = publisher.PostId;
    }

    public Publisher ToDomain()
    {
        return Publisher.Restore(PublisherId, PostId);
    }

    public static PublisherEntity ToEntity(Publisher domain)
    {
        return new PublisherEntity(domain);
    }
}