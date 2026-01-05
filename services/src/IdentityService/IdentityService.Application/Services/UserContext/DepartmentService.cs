using IdentityService.Domain.Abstractions.Application.Services.UserContext;
using IdentityService.Domain.Abstractions.Infrastructure.Repositories.UserContext;
using IdentityService.Domain.CustomExceptions;
using IdentityService.Domain.Models.UserContext;

namespace IdentityService.Application.Services.UserContext;

public class DepartmentService : IDepartmentService
{
    private readonly IDepartmentRepository _departmentRepository;

    public DepartmentService(IDepartmentRepository departmentRepository)
    {
        _departmentRepository = departmentRepository;
    }

    public async Task<List<Department>> GetAllDepartment(CancellationToken cancellationToken = default)
    {
        return await _departmentRepository.GetAll(cancellationToken);
    }

    public async Task<Department?> GetDepartmentById(int id, CancellationToken cancellationToken = default)
    {
        return await _departmentRepository.GetById(id, cancellationToken);
    }

    public async Task<Department> CreateDepartment(string title, CancellationToken cancellationToken = default)
    {
        Department department = Department.CreateDepartment(title);

        return await _departmentRepository.Create(department, cancellationToken);
    }

    public async Task<bool> UpdateDepartment(int departmentId, string newDepartmentTitle, CancellationToken cancellationToken = default)
    {
        Department? department = await _departmentRepository.FindAsync(departmentId, cancellationToken);
        
        if (department is null)
            throw new NotFoundException("Отдел", departmentId);
        
        department.UpdateDepartment(newDepartmentTitle);

        return await _departmentRepository.Update(department, cancellationToken);
    }

    public async Task<bool> DeleteDepartment(int departmentId, CancellationToken cancellationToken = default)
    {
        return await _departmentRepository.Delete(departmentId, cancellationToken);
    }
}