using IdentityService.Domain.Models.UserContext;

namespace IdentityService.Domain.Abstractions.Application.Services.UserContext;

public interface IGroupService
{
    Task<List<Group>> GetAllGroup(CancellationToken cancellationToken = default);

    Task<List<Group>> GetAllGroupSpecialtyId(
        int specialtyId,
        CancellationToken cancellationToken = default
    );

    Task<Group?> GetGroupById(int id, CancellationToken cancellationToken = default);

    Task<Group> CreateGroup(
        int course,
        int numberGroup,
        int specialtyId,
        CancellationToken cancellationToken = default
    );

    Task<bool> UpdateGroup(
        int groupId,
        int newCourse,
        int newNumberGroup,
        int newSpecialtyId,
        CancellationToken cancellationToken = default
    );

    Task<bool> DeleteGroup(int groupId, CancellationToken cancellationToken = default);
}
