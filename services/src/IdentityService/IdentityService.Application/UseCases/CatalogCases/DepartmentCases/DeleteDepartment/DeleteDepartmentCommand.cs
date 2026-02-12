using IdentityService.Domain.Result;
using MediatR;

namespace IdentityService.Application.UseCases.CatalogCases.DepartmentCases.DeleteDepartment;

public record DeleteDepartmentCommand(int DepartmentId) : IRequest<Result<int>>;
