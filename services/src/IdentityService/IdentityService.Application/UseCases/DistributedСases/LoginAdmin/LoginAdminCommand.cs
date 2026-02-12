using IdentityService.Application.Contracts;
using IdentityService.Domain.Result;
using MediatR;

namespace IdentityService.Application.UseCases.DistributedСases.LoginAdmin;

public record LoginAdminCommand(string Email, string Password) : IRequest<Result<LoginResponse>>;
