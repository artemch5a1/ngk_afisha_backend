using IdentityService.Domain.Models.UserContext;
using IdentityService.Domain.Result;
using MediatR;

namespace IdentityService.Application.UseCases.CatalogCases.DepartmentCases.CreateDepartment;

public record CreateDepartmentCommand(string DepartmentTitle) : IRequest<Result<Department>>;