using IdentityService.Domain.Abstractions.Application.Services.UserContext;
using IdentityService.Domain.Abstractions.Infrastructure.Repositories.UserContext;
using IdentityService.Domain.CustomExceptions;
using IdentityService.Domain.Models.UserContext;

namespace IdentityService.Application.Services.UserContext;

public class GroupService : IGroupService
{
    private readonly IGroupRepository _groupRepository;
    
    public GroupService(IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public async Task<List<Group>> GetAllGroup(CancellationToken cancellationToken = default)
    {
        return await _groupRepository.GetAll(cancellationToken);
    }

    public async Task<List<Group>> GetAllGroupSpecialtyId(int specialtyId, CancellationToken cancellationToken = default)
    {
        return await _groupRepository.GetAllBySpecialtyId(specialtyId, cancellationToken);
    }
    
    public async Task<Group?> GetGroupById(int id, CancellationToken cancellationToken = default)
    {
        return await _groupRepository.GetById(id, cancellationToken);
    }

    public async Task<Group> CreateGroup(int course, int numberGroup, int specialtyId, CancellationToken cancellationToken = default)
    {
        Group group = Group.CreateGroup(course, numberGroup, specialtyId);

        return await _groupRepository.Create(group, cancellationToken);
    }

    public async Task<bool> UpdateGroup(int groupId, int newCourse, int newNumberGroup, int newSpecialtyId, CancellationToken cancellationToken = default)
    {
        Group? group = await _groupRepository.FindAsync(groupId, cancellationToken);

        if (group is null)
            throw new NotFoundException("Группа", groupId);

        group.UpdateGroup(newCourse, newNumberGroup, newSpecialtyId);

        return await _groupRepository.Update(group, cancellationToken);
    }

    public async Task<bool> DeleteGroup(int groupId, CancellationToken cancellationToken = default)
    {
        return await _groupRepository.Delete(groupId, cancellationToken);
    }
}