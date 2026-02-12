using IdentityService.Domain.Abstractions.Infrastructure.Utils;

namespace IdentityService.Infrastructure.Implementations.Utils;

public class BCryptHasher : IPasswordHasher
{
    public string HashPassword(string password) =>
        BCrypt.Net.BCrypt.EnhancedHashPassword(password)!;

    public bool VerifyPassword(string password, string passwordHash) =>
        BCrypt.Net.BCrypt.EnhancedVerify(password, passwordHash);
}
