using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using IdentityService.Domain.Abstractions.Infrastructure.Providers;
using IdentityService.Domain.Enums;
using IdentityService.Domain.Extensions;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace IdentityService.Infrastructure.Implementations.Providers.AccessTokenProvider;

public class JwtProvider : IAccessTokenProvider
{
    private readonly IOptions<JwtOptions> _options;
    
    private readonly RSA _rsa;
    
    public JwtProvider(IOptions<JwtOptions> options)
    {
        _options = options;
        _rsa = CreateRsaFromPrivateKey(_options.Value.PrivateKey);
    }
    
    public string GenerateToken(Guid accountId, string email, Role role)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, accountId.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, email),
            new Claim(ClaimTypes.Role, role.GetString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(_options.Value.ExpiresMinutes),
            Issuer = _options.Value.Issuer,
            SigningCredentials = new SigningCredentials(new RsaSecurityKey(_rsa), SecurityAlgorithms.RsaSha256)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
    
    public bool ValidateToken(string token)
    {
        try
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var rsa = CreateRsaFromPublicKey(_options.Value.PublicKey);
            
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = _options.Value.Issuer,
                ValidateAudience = false,
                ValidateLifetime = true,
                IssuerSigningKey = new RsaSecurityKey(rsa),
                ValidateIssuerSigningKey = true
            };

            tokenHandler.ValidateToken(token, validationParameters, out _);
            return true;
        }
        catch
        {
            return false;
        }
    }
    
    private RSA CreateRsaFromPrivateKey(string privateKey)
    {
        var rsa = RSA.Create();
        
        try
        {
            var keyBytes = Convert.FromBase64String(privateKey
                .Replace("-----BEGIN RSA PRIVATE KEY-----", "")
                .Replace("-----END RSA PRIVATE KEY-----", "")
                .Replace("-----BEGIN PRIVATE KEY-----", "")
                .Replace("-----END PRIVATE KEY-----", "")
                .Replace("\n", "")
                .Replace("\r", "")
                .Trim());

            rsa.ImportPkcs8PrivateKey(keyBytes, out _);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Invalid private key format", ex);
        }

        return rsa;
    }
    
    private RSA CreateRsaFromPublicKey(string publicKey)
    {
        var rsa = RSA.Create();
        
        try
        {
            var keyBytes = Convert.FromBase64String(publicKey
                .Replace("-----BEGIN RSA PRIVATE KEY-----", "")
                .Replace("-----END RSA PRIVATE KEY-----", "")
                .Replace("-----BEGIN PRIVATE KEY-----", "")
                .Replace("-----END PRIVATE KEY-----", "")
                .Replace("\n", "")
                .Replace("\r", "")
                .Trim());

            rsa.ImportSubjectPublicKeyInfo(keyBytes, out _);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Invalid public key format", ex);
        }

        return rsa;
    }
}