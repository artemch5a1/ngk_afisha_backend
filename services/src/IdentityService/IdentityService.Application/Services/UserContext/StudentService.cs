using IdentityService.Domain.Abstractions.Application.Services.UserContext;
using IdentityService.Domain.Abstractions.Infrastructure.Repositories.UserContext;
using IdentityService.Domain.Models.UserContext;

namespace IdentityService.Application.Services.UserContext;

public class StudentService : IStudentService
{
    private readonly IStudentRepository _repository;

    public StudentService(IStudentRepository repository)
    {
        _repository = repository;
    }


    public async Task<List<Student>> GetAllStudent(CancellationToken cancellationToken = default)
    {
        return await _repository.GetAll(cancellationToken);
    }

    public async Task<Student?> GetStudentById(Guid studentId, CancellationToken cancellationToken = default)
    {
        return await _repository.GetById(studentId, cancellationToken);
    }
}