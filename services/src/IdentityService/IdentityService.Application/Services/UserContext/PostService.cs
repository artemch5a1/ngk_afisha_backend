using IdentityService.Domain.Abstractions.Application.Services.UserContext;
using IdentityService.Domain.Abstractions.Infrastructure.Repositories.UserContext;
using IdentityService.Domain.CustomExceptions;
using IdentityService.Domain.Models.UserContext;

namespace IdentityService.Application.Services.UserContext;

public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;

    public PostService(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task<List<Post>> GetAllPost(CancellationToken cancellationToken = default)
    {
        return await _postRepository.GetAll(cancellationToken);
    }

    public async Task<Post?> GetPostById(int id, CancellationToken cancellationToken = default)
    {
        return await _postRepository.GetById(id, cancellationToken);
    }

    public async Task<List<Post>> GetAllPostByDepartmentId(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        return await _postRepository.GetAllPostByDepartmentId(id, cancellationToken);
    }

    public async Task<Post> CreatePost(
        int departmentId,
        string title,
        CancellationToken cancellationToken = default
    )
    {
        Post post = Post.Create(title, departmentId);

        return await _postRepository.Create(post, cancellationToken);
    }

    public async Task<bool> UpdatePost(
        int postId,
        string newTitle,
        int newDepartmentId,
        CancellationToken cancellationToken = default
    )
    {
        Post? post = await _postRepository.FindAsync(postId, cancellationToken);

        if (post is null)
            throw new NotFoundException("Должность", postId);

        post.UpdatePost(newTitle, newDepartmentId);

        return await _postRepository.Update(post, cancellationToken);
    }

    public async Task<bool> DeletePost(int postId, CancellationToken cancellationToken = default)
    {
        return await _postRepository.Delete(postId, cancellationToken);
    }
}
