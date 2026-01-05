using IdentityService.Application.Contracts;
using IdentityService.Domain.Result;
using MediatR;

namespace IdentityService.Application.UseCases.DistributedСases.Login;

public record LoginCommand(string Email, string Password) : IRequest<Result<LoginResponse>>;