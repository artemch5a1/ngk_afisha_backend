namespace IdentityService.Infrastructure.Implementations.Providers.AccessTokenProvider;

public class JwtOptions
{
    public string PrivateKey { get; set; } = null!;

    public string PublicKey { get; set; } = null!;

    public string Issuer { get; set; } = null!;

    public int ExpiresMinutes { get; set; } = 120;
}
