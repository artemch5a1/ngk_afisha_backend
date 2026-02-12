using IdentityService.Domain.Models.UserContext;

namespace IdentityService.Domain.Abstractions.Application.Services.UserContext;

public interface IDepartmentService
{
    Task<List<Department>> GetAllDepartment(CancellationToken cancellationToken = default);

    Task<Department?> GetDepartmentById(int id, CancellationToken cancellationToken = default);

    Task<Department> CreateDepartment(string title, CancellationToken cancellationToken = default);

    Task<bool> UpdateDepartment(
        int departmentId,
        string newDepartmentTitle,
        CancellationToken cancellationToken = default
    );

    Task<bool> DeleteDepartment(int departmentId, CancellationToken cancellationToken = default);
}
