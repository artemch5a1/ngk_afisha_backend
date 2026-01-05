using IdentityService.Domain.Models.UserContext;
using IdentityService.Domain.Result;
using MediatR;

namespace IdentityService.Application.UseCases.CatalogCases.GroupCases.GetGroupById;

public record GetGroupByIdQuery(int GroupId) : IRequest<Result<Group>>;