using IdentityService.Domain.Models.UserContext;

namespace IdentityService.Domain.Abstractions.Application.Services.UserContext;

public interface IUserService
{

    Task<User?> GetUserById(Guid id, CancellationToken ct = default);
    
    Task<List<User>> GetAllUsers(CancellationToken ct = default);
    
    Task<User> CreateStudent(
        Guid userId,
        string surname, 
        string name, 
        string? patronymic, 
        DateOnly dateBirth,
        int groupId,
        CancellationToken ct = default);


    Task<User> CreatePublisher(
        Guid userId,
        string surname, 
        string name, 
        string? patronymic, 
        DateOnly dateBirth,
        int postId,
        CancellationToken ct = default
        );
    
    Task<bool> UpdateUserInfo(
        Guid userId,
        string surname, 
        string name, 
        string? patronymic, 
        DateOnly dateBirth);

    Task<bool> UpdateStudentProfile(Guid userId, int groupId);

    Task<bool> UpdatePublisherProfile(Guid userId, int postId);
}