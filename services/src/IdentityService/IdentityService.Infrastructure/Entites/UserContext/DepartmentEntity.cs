using System.ComponentModel.DataAnnotations.Schema;
using IdentityService.Domain.Abstractions.Infrastructure.Entity;
using IdentityService.Domain.Models.UserContext;

namespace IdentityService.Infrastructure.Entites.UserContext;

public class DepartmentEntity : IEntity<DepartmentEntity, Department>
{
    [Column("department_id")]
    public int DepartmentId { get; set; }

    [Column("title")]
    public string Title { get; set; } = null!;

    public List<PostEntity> Posts { get; set; } = null!;

    internal DepartmentEntity() { }

    private DepartmentEntity(Department department)
    {
        Title = department.Title;
    }

    public Department ToDomain()
    {
        return Department.Restore(DepartmentId, Title);
    }

    public static DepartmentEntity ToEntity(Department domain)
    {
        return new DepartmentEntity(domain);
    }
}
