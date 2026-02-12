namespace IdentityService.API.Contracts.AccountActions;

public class RegistryStudentDto
{
    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Surname { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Patronymic { get; set; }

    public DateOnly DateBirth { get; set; }

    public int GroupId { get; set; }
}
