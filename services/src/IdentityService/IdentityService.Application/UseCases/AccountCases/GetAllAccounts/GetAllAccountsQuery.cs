using IdentityService.Domain.Models.AccountContext;
using IdentityService.Domain.Result;
using MediatR;

namespace IdentityService.Application.UseCases.AccountCases.GetAllAccounts;

public record GetAllAccountsQuery() : IRequest<Result<List<Account>>>;