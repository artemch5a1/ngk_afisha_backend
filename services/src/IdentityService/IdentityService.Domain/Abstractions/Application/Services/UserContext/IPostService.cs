using IdentityService.Domain.Models.UserContext;

namespace IdentityService.Domain.Abstractions.Application.Services.UserContext;

public interface IPostService
{
    Task<List<Post>> GetAllPost(CancellationToken cancellationToken = default);

    Task<Post?> GetPostById(int id, CancellationToken cancellationToken = default);

    Task<List<Post>> GetAllPostByDepartmentId(int id, CancellationToken cancellationToken = default);

    Task<Post> CreatePost(int departmentId, string title, CancellationToken cancellationToken = default);

    Task<bool> UpdatePost(int postId, string newTitle, int newDepartmentId, CancellationToken cancellationToken = default);

    Task<bool> DeletePost(int postId, CancellationToken cancellationToken = default);
}