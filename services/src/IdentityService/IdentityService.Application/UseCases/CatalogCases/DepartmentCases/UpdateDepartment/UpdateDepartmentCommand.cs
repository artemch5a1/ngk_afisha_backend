using IdentityService.Domain.Result;
using MediatR;

namespace IdentityService.Application.UseCases.CatalogCases.DepartmentCases.UpdateDepartment;

public record UpdateDepartmentCommand(int DepartmentId, string DepartmentTitle)
    : IRequest<Result<int>>;
