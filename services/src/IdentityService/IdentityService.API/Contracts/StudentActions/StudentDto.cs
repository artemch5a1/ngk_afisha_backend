using IdentityService.API.Contracts.GroupActions;
using IdentityService.API.Contracts.UserActions;

namespace IdentityService.API.Contracts.StudentActions;

public class StudentDto
{
    public Guid StudentId { get; set; }

    public UserDto User { get; set; } = null!;

    public int GroupId { get; set; }

    public GroupDto Group { get; set; } = null!;
}