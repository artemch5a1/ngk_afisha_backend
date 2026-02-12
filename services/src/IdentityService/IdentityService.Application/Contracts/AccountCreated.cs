using IdentityService.Domain.Enums;

namespace IdentityService.Application.Contracts;

public record AccountCreated(Guid AccountId, string Email, Role Role);
