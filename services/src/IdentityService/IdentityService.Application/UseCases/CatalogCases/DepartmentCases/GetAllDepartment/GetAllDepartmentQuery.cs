using IdentityService.Domain.Models.UserContext;
using IdentityService.Domain.Result;
using MediatR;

namespace IdentityService.Application.UseCases.CatalogCases.DepartmentCases.GetAllDepartment;

public record GetAllDepartmentQuery() : IRequest<Result<List<Department>>>;
