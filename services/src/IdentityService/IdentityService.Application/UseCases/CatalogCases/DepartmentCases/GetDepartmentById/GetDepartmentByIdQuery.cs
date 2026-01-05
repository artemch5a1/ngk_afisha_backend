using IdentityService.Domain.Models.UserContext;
using IdentityService.Domain.Result;
using MediatR;

namespace IdentityService.Application.UseCases.CatalogCases.DepartmentCases.GetDepartmentById;

public record GetDepartmentByIdQuery(int DepartmentId) : IRequest<Result<Department>>;