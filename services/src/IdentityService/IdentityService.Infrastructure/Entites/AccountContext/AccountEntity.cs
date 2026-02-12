using System.ComponentModel.DataAnnotations.Schema;
using IdentityService.Domain.Abstractions.Infrastructure.Entity;
using IdentityService.Domain.Enums;
using IdentityService.Domain.Models.AccountContext;

namespace IdentityService.Infrastructure.Entites.AccountContext;

public class AccountEntity : IEntity<AccountEntity, Account>
{
    [Column("account_id")]
    public Guid AccountId { get; set; }

    [Column("email")]
    public string Email { get; set; } = null!;

    [Column("password_hash")]
    public string PasswordHash { get; set; } = null!;

    [Column("created_date")]
    public DateTime CreatedDate { get; private set; }

    [Column("role")]
    public int Role { get; set; }

    internal AccountEntity() { }

    internal AccountEntity(string email, string passwordHash)
    {
        Email = email;
        PasswordHash = passwordHash;
        CreatedDate = DateTime.UtcNow;
        Role = (int)Domain.Enums.Role.Admin;
    }

    public AccountEntity(Account account)
    {
        AccountId = account.AccountId;
        Email = account.Email;
        PasswordHash = account.PasswordHash;
        CreatedDate = account.CreatedDate;
        Role = (int)account.AccountRole;
    }

    public Account ToDomain()
    {
        return Account.Restore(AccountId, Email, PasswordHash, CreatedDate, (Role)Role);
    }

    public static AccountEntity ToEntity(Account domain)
    {
        return new AccountEntity(domain);
    }
}
