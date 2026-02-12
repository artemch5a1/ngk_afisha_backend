using IdentityService.Domain.Models.UserContext;
using IdentityService.Domain.Result;
using MediatR;

namespace IdentityService.Application.UseCases.CatalogCases.PostCases.GetAllPostByDepartment;

public record GetAllPostByDepartmentQuery(int DepartmentId) : IRequest<Result<List<Post>>>;
