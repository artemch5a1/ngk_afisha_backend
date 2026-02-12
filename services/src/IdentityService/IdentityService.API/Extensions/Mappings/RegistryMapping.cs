using IdentityService.API.Contracts.AccountActions;
using IdentityService.Application.Contracts;
using IdentityService.Application.UseCases.DistributedСases.RegistryPublisher;
using IdentityService.Application.UseCases.DistributedСases.RegistryStudent;

namespace IdentityService.API.Extensions.Mappings;

public static class RegistryMapping
{
    public static RegistryStudentCommand ToCommand(this RegistryStudentDto dto) =>
        new(
            dto.Email,
            dto.Password,
            dto.Surname,
            dto.Name,
            dto.Patronymic,
            dto.DateBirth,
            dto.GroupId
        );

    public static CreatedAccountDto ToDto(this AccountCreated created) =>
        new()
        {
            AccountId = created.AccountId,
            Email = created.Email,
            Role = created.Role,
        };

    public static RegistryPublisherCommand ToCommand(this RegistryPublisherDto dto) =>
        new RegistryPublisherCommand(
            dto.Email,
            dto.Password,
            dto.Surname,
            dto.Name,
            dto.Patronymic,
            dto.DateBirth,
            dto.PostId
        );
}
