using IdentityService.Domain.Abstractions.Application.Services.UserContext;
using IdentityService.Domain.Abstractions.Infrastructure.Repositories.UserContext;
using IdentityService.Domain.CustomExceptions;
using IdentityService.Domain.Models.UserContext;

namespace IdentityService.Application.Services.UserContext;

public class UserService : IUserService
{
    private readonly IUserRepository _userRepository;

    public UserService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<List<User>> GetAllUsers(CancellationToken ct = default)
    {
        return await _userRepository.GetAll(ct);
    }

    public async Task<User?> GetUserById(Guid id, CancellationToken ct = default)
    {
        return await _userRepository.GetById(id, ct);
    }

    public async Task<User> CreateStudent(
        Guid userId,
        string surname,
        string name,
        string? patronymic,
        DateOnly dateBirth,
        int groupId,
        CancellationToken ct = default
    )
    {
        User user = User.CreateStudent(userId, surname, name, patronymic, dateBirth, groupId);

        User createdUser = await _userRepository.Create(user, ct);

        return createdUser;
    }

    public async Task<User> CreatePublisher(
        Guid userId,
        string surname,
        string name,
        string? patronymic,
        DateOnly dateBirth,
        int postId,
        CancellationToken ct = default
    )
    {
        User user = User.CreatePublisher(userId, surname, name, patronymic, dateBirth, postId);

        User createdUser = await _userRepository.Create(user, ct);

        return createdUser;
    }

    public async Task<bool> UpdateUserInfo(
        Guid userId,
        string surname,
        string name,
        string? patronymic,
        DateOnly dateBirth
    )
    {
        User? user = await _userRepository.FindAsync(userId);

        if (user == null)
            throw new NotFoundException("Пользователь", userId);

        user.UpdateFields(surname, name, patronymic, dateBirth);

        return await _userRepository.Update(user);
    }

    public async Task<bool> UpdateStudentProfile(Guid userId, int groupId)
    {
        User? user = await _userRepository.GetById(userId);

        if (user is null)
            throw new NotFoundException("Пользователь", userId);

        user.UpdateStudentProfile(groupId);

        return await _userRepository.UpdateStudentProfile(user);
    }

    public async Task<bool> UpdatePublisherProfile(Guid userId, int postId)
    {
        User? user = await _userRepository.GetById(userId);

        if (user is null)
            throw new NotFoundException("Пользователь", userId);

        user.UpdatePublisherProfile(postId);

        return await _userRepository.UpdatePublisherProfile(user);
    }
}
