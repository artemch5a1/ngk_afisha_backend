namespace IdentityService.API.Contracts.PostActions;

public class CreatePostDto
{
    public string Title { get; set; } = null!;
    
    public int DepartmentId { get; set; }
}