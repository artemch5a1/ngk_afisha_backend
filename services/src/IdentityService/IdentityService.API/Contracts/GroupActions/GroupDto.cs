using IdentityService.API.Contracts.SpecialtyActions;

namespace IdentityService.API.Contracts.GroupActions;

public class GroupDto
{
    public int GroupId { get; set; }

    public int Course { get; set; }

    public int NumberGroup { get; set; }
    
    public int SpecialtyId { get; set;  }

    public SpecialtyDto Specialty { get; set; } = null!;

    public string FullName { get; set; } = null!;
}