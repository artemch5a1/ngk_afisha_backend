namespace IdentityService.API.Contracts.DepartmentActions;

public class UpdateDepartmentDto
{
    public int DepartmentId { get; set; }

    public string NewTitle { get; set; } = null!;
}