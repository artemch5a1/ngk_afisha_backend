using IdentityService.API.Contracts.GroupActions;
using IdentityService.Application.UseCases.CatalogCases.GroupCases.CreateGroup;
using IdentityService.Application.UseCases.CatalogCases.GroupCases.UpdateGroup;
using IdentityService.Domain.Models.UserContext;

namespace IdentityService.API.Extensions.Mappings;

public static class GroupMapping
{
    public static GroupDto ToDto(this Group group)
    {
        GroupDto dto = new GroupDto()
        {
            GroupId = group.GroupId,
            Course = group.Course,
            NumberGroup = group.NumberGroup,
            SpecialtyId = group.SpecialtyId,

            FullName = group.GetIdentityGroup(),
        };

        if (group.Specialty is not null)
            dto.Specialty = group.Specialty.ToDto();

        return dto;
    }

    public static List<GroupDto> ToListDto(this List<Group> groups)
    {
        return groups.Select(x => x.ToDto()).ToList();
    }

    public static CreateGroupCommand ToCommand(this CreateGroupDto dto)
    {
        return new CreateGroupCommand(dto.Course, dto.NumberGroup, dto.SpecialtyId);
    }

    public static UpdateGroupCommand ToCommand(this UpdateGroupDto dto)
    {
        return new UpdateGroupCommand(dto.GroupId, dto.Course, dto.NumberGroup, dto.SpecialtyId);
    }
}
