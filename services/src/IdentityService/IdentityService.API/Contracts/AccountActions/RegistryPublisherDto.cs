namespace IdentityService.API.Contracts.AccountActions;

public class RegistryPublisherDto
{
    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Surname { get; set; } = null!;
    
    public string Name { get; set; } = null!;
    
    public string? Patronymic { get; set; }
    
    public DateOnly DateBirth { get; set; }

    public int PostId { get; set; }
}