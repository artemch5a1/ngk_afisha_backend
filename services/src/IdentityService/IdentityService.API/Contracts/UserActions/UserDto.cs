using IdentityService.API.Contracts.StudentActions;

namespace IdentityService.API.Contracts.UserActions;

public class UserDto
{
    public Guid UserId { get; set; }

    public string Surname { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Patronymic { get; set; }

    public DateOnly BirthDate { get; set; }
}
