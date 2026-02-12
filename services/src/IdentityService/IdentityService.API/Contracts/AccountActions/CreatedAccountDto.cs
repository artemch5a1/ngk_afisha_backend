using IdentityService.Domain.Enums;

namespace IdentityService.API.Contracts.AccountActions;

public class CreatedAccountDto
{
    public Guid AccountId { get; set; }

    public string Email { get; set; } = null!;

    public Role Role { get; set; }
}
