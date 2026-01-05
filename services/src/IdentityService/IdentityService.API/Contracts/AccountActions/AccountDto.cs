using IdentityService.Domain.Enums;

namespace IdentityService.API.Contracts.AccountActions;

public class AccountDto
{
    public Guid AccountId { get; set; }

    public string Email { get; set; } = null!;

    public Role AccountRole { get; set; }
}