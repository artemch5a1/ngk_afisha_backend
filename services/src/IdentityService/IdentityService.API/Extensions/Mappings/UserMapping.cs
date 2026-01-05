using IdentityService.API.Contracts.UserActions;
using IdentityService.Application.UseCases.UserCases.UpdatePublisherProfile;
using IdentityService.Application.UseCases.UserCases.UpdateStudentProfile;
using IdentityService.Application.UseCases.UserCases.UpdateUser;
using IdentityService.Domain.Models.UserContext;

namespace IdentityService.API.Extensions.Mappings;

public static class UserMapping
{
    public static UserDto ToDto(this User user)
    {
        UserDto dto = new UserDto()
        {
            UserId = user.UserId,
            Surname = user.Surname,
            Name = user.Name,
            Patronymic = user.Patronymic,
            BirthDate = user.BirthDate
        };

        return dto;
    }
    
    public static List<UserDto> ToListDto(this List<User> users)
    {
        return users.Select(x => x.ToDto()).ToList();
    }

    public static UpdateUserCommand ToCommand(this UpdateUserDto dto, Guid userId)
    {
        return new UpdateUserCommand(userId, dto.Surname, dto.Name, dto.Patronymic, dto.DateBirth);
    }

    public static UpdateStudentProfileCommand ToCommand(this UpdateStudentProfileDto dto, Guid studentId)
    {
        return new UpdateStudentProfileCommand(studentId, dto.NewGroupId);
    }
    
    public static UpdatePublisherProfileCommand ToCommand(this UpdatePublisherProfileDto dto, Guid studentId)
    {
        return new UpdatePublisherProfileCommand(studentId, dto.NewPostId);
    }
}