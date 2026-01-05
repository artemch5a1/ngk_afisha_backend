using System.ComponentModel.DataAnnotations.Schema;
using IdentityService.Domain.Abstractions.Infrastructure.Entity;
using IdentityService.Domain.Models.UserContext;

namespace IdentityService.Infrastructure.Entites.UserContext;

public class PostEntity : IEntity<PostEntity, Post>
{
    [Column("post_id")]
    public int PostId { get; set; }

    [Column("title")]
    public string Title { get; set; } = null!;
    
    [Column("department_id")]
    public int DepartmentId { get; set; }
    
    [ForeignKey(nameof(DepartmentId))]
    public DepartmentEntity Department { get; set; } = null!;

    internal PostEntity()
    {
        
    }
    
    private PostEntity(Post domain)
    {
        Title = domain.Title;
        DepartmentId = domain.DepartmentId;
    }

    public Post ToDomain()
    {
        return Post.Restore(PostId, Title, DepartmentId);
    }

    public static PostEntity ToEntity(Post domain)
    {
        return new PostEntity(domain);
    }
}