using IdentityService.API.Contracts.DepartmentActions;
using IdentityService.Application.UseCases.CatalogCases.DepartmentCases.CreateDepartment;
using IdentityService.Application.UseCases.CatalogCases.DepartmentCases.UpdateDepartment;
using IdentityService.Domain.Models.UserContext;

namespace IdentityService.API.Extensions.Mappings;

public static class DepartmentMappings
{
    public static DepartmentDto ToDto(this Department department)
    {
        return new DepartmentDto()
        {
            DepartmentId = department.DepartmentId,
            Title = department.Title
        };
    }

    public static List<DepartmentDto> ToListDto(this List<Department> departments)
    {
        return departments.Select(x => x.ToDto()).ToList();
    }

    public static CreateDepartmentCommand ToCommand(this CreateDepartmentDto dto)
    {
        return new CreateDepartmentCommand(dto.Title);
    }
    
    public static UpdateDepartmentCommand ToCommand(this UpdateDepartmentDto dto)
    {
        return new UpdateDepartmentCommand(dto.DepartmentId, dto.NewTitle);
    }
}