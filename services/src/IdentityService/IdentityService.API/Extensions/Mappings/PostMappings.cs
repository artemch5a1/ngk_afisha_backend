using IdentityService.API.Contracts.PostActions;
using IdentityService.Application.UseCases.CatalogCases.PostCases.CreatePost;
using IdentityService.Application.UseCases.CatalogCases.PostCases.UpdatePost;
using IdentityService.Domain.Models.UserContext;

namespace IdentityService.API.Extensions.Mappings;

public static class PostMappings
{
    public static PostDto ToDto(this Post post)
    {
        PostDto postDto = new PostDto()
        {
            PostId = post.PostId,
            Title = post.Title,
            DepartmentId = post.DepartmentId,
        };

        if (post.Department is not null)
            postDto.Department = post.Department.ToDto();

        return postDto;
    }

    public static List<PostDto> ToListDto(this List<Post> departments)
    {
        return departments.Select(x => x.ToDto()).ToList();
    }

    public static CreatePostCommand ToCommand(this CreatePostDto dto)
    {
        return new CreatePostCommand(dto.Title, dto.DepartmentId);
    }

    public static UpdatePostCommand ToCommand(this UpdatePostDto dto)
    {
        return new UpdatePostCommand(dto.PostId, dto.Title, dto.DepartmentId);
    }
}
