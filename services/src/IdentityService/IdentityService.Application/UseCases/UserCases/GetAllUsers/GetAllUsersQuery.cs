using IdentityService.Domain.Models.UserContext;
using IdentityService.Domain.Result;
using MediatR;

namespace IdentityService.Application.UseCases.UserCases.GetAllUsers;

public record GetAllUsersQuery() : IRequest<Result<List<User>>>;
