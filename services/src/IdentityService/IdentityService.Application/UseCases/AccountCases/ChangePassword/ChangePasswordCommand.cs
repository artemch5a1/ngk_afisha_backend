using IdentityService.Domain.Result;
using MediatR;

namespace IdentityService.Application.UseCases.AccountCases.ChangePassword;

public record ChangePasswordCommand(Guid AccountId, string OldPassword, string NewPassword) : IRequest<Result<Guid>>;