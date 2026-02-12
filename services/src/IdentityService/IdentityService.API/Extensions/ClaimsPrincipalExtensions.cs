using System.Security.Claims;
using IdentityService.Domain.Enums;
using IdentityService.Domain.Result;

namespace IdentityService.API.Extensions;

public static class ClaimsPrincipalExtensions
{
    public static Result<Guid> ExtractGuid(this ClaimsPrincipal user)
    {
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
        {
            return Result<Guid>.Failure(["Неверный токен"], ApiErrorType.Forbidden);
        }

        return Result<Guid>.Success(userGuid);
    }

    public static bool VerifyGuid(this ClaimsPrincipal user, Guid expectedGuid)
    {
        var userId = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var userGuid))
            return false;

        return userGuid == expectedGuid;
    }
}
