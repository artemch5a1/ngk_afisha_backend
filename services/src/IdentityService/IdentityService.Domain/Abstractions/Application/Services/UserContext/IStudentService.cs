using IdentityService.Domain.Models.UserContext;

namespace IdentityService.Domain.Abstractions.Application.Services.UserContext;

public interface IStudentService
{
    Task<List<Student>> GetAllStudent(CancellationToken cancellationToken = default);

    Task<Student?> GetStudentById(Guid studentId, CancellationToken cancellationToken = default);
}