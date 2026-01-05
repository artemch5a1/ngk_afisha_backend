namespace IdentityService.API.Contracts.UserActions;

public class UpdateUserDto
{
    public string Surname { get; set; } = null!;
    
    public string Name { get; set; } = null!;
    
    public string? Patronymic { get; set; }
    
    public DateOnly DateBirth { get; set; }
}