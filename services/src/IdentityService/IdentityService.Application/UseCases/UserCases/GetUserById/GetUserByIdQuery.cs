using IdentityService.Domain.Models.UserContext;
using IdentityService.Domain.Result;
using MediatR;

namespace IdentityService.Application.UseCases.UserCases.GetUserById;

public record GetUserByIdQuery(Guid UserId) : IRequest<Result<User>>;