using IdentityService.Domain.Models.UserContext;
using IdentityService.Domain.Result;
using MediatR;

namespace IdentityService.Application.UseCases.CatalogCases.GroupCases.GetAllGroup;

public record GetAllGroupQuery : IRequest<Result<List<Group>>>;