namespace IdentityService.API.Contracts.SpecialtyActions;

public class UpdateSpecialtyDto
{
    public int SpecialtyId { get; set; }

    public string SpecialtyTitle { get; set; } = null!;
}