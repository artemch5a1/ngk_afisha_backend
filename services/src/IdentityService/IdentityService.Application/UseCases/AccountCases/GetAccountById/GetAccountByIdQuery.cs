using IdentityService.Domain.Models.AccountContext;
using IdentityService.Domain.Result;
using MediatR;

namespace IdentityService.Application.UseCases.AccountCases.GetAccountById;

public record GetAccountByIdQuery(Guid AccountId) : IRequest<Result<Account>>;
