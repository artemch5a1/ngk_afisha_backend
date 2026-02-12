using IdentityService.API.Contracts.AccountActions;
using IdentityService.Application.Contracts;
using IdentityService.Application.UseCases.AccountCases.ChangePassword;
using IdentityService.Application.UseCases.DistributedСases.Login;
using IdentityService.Application.UseCases.DistributedСases.LoginAdmin;
using IdentityService.Domain.Models.AccountContext;

namespace IdentityService.API.Extensions.Mappings;

public static class AccountMapping
{
    public static AccountDto ToDto(this Account account)
    {
        return new AccountDto()
        {
            AccountId = account.AccountId,
            Email = account.Email,
            AccountRole = account.AccountRole,
        };
    }

    public static List<AccountDto> ToListDto(this List<Account> accounts)
    {
        return accounts.Select(x => x.ToDto()).ToList();
    }

    public static LoginCommand ToCommand(this LoginRequest request)
    {
        return new LoginCommand(request.Email, request.Password);
    }

    public static LoginAdminCommand ToCommand(this LoginAdminRequest request)
    {
        return new LoginAdminCommand(request.Email, request.Password);
    }

    public static ChangePasswordCommand ToCommand(this ChangePasswordDto dto, Guid accountId)
    {
        return new ChangePasswordCommand(accountId, dto.OldPassword, dto.NewPassword);
    }

    public static LoginResponseDto ToDto(this LoginResponse response)
    {
        return new LoginResponseDto()
        {
            AccountId = response.AccountId,
            Email = response.Email,
            Role = response.Role,
            AccessToken = response.AccessToken,
        };
    }
}
