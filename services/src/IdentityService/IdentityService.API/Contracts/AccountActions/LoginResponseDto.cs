using IdentityService.Domain.Enums;

namespace IdentityService.API.Contracts.AccountActions;

public class LoginResponseDto
{
    public Guid AccountId { get; set; }

    public string Email { get; set; } = null!;
    
    public Role Role { get; set; }
    
    public string AccessToken { get; set; } = null!;
} 
