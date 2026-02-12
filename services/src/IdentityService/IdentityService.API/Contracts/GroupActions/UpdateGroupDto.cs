namespace IdentityService.API.Contracts.GroupActions;

public class UpdateGroupDto
{
    public int GroupId { get; set; }

    public int Course { get; set; }

    public int NumberGroup { get; set; }

    public int SpecialtyId { get; set; }
}
