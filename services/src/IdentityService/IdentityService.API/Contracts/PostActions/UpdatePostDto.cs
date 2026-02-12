namespace IdentityService.API.Contracts.PostActions;

public class UpdatePostDto
{
    public int PostId { get; set; }

    public string Title { get; set; } = null!;

    public int DepartmentId { get; set; }
}
