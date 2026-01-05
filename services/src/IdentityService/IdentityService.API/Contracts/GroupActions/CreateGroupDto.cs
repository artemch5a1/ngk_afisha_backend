namespace IdentityService.API.Contracts.GroupActions;

public class CreateGroupDto
{
    public int Course { get; set; }

    public int NumberGroup { get; set; }
    
    public int SpecialtyId { get; set;  }
}