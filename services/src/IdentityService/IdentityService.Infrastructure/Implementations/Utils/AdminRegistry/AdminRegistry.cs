using IdentityService.Domain.Abstractions.Application.Services.StartupService;
using IdentityService.Domain.Abstractions.Infrastructure.Utils;
using IdentityService.Domain.Enums;
using IdentityService.Infrastructure.Data.Database;
using IdentityService.Infrastructure.Entites.AccountContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace IdentityService.Infrastructure.Implementations.Utils.AdminRegistry;

public class AdminRegistry : IStartupService
{
    private readonly IdentityServiceDbContext _db;

    private readonly AdminCred _adminCred;

    private readonly IPasswordHasher _passwordHasher;

    public AdminRegistry(
        IdentityServiceDbContext db,
        IOptions<AdminCred> adminCred,
        IPasswordHasher passwordHasher
    )
    {
        _db = db;
        _adminCred = adminCred.Value;
        _passwordHasher = passwordHasher;
    }

    private async Task RegistryAdmin(CancellationToken ct = default)
    {
        bool isExist = await _db.Accounts.AnyAsync(x => x.Email == _adminCred.Email, ct);

        if (isExist)
            return;

        await _db.Accounts.Where(x => x.Role == (int)Role.Admin).ExecuteDeleteAsync(ct);

        string passwordHah = _passwordHasher.HashPassword(_adminCred.Password);

        AccountEntity accountAdmin = new AccountEntity(_adminCred.Email, passwordHah);

        await _db.Accounts.AddAsync(accountAdmin, ct);

        await _db.SaveChangesAsync(ct);
    }

    public int Order => 1;

    public async Task InvokeAsync(CancellationToken ct = default)
    {
        await RegistryAdmin(ct);
    }
}
