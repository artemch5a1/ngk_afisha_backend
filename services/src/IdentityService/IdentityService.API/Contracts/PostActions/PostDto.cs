using IdentityService.API.Contracts.DepartmentActions;

namespace IdentityService.API.Contracts.PostActions;

public class PostDto
{
    public int PostId { get; set; }

    public string Title { get; set; } = null!;

    public int DepartmentId { get; set; }

    public DepartmentDto Department { get; set; } = null!;
}
