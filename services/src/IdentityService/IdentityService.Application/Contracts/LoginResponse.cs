using IdentityService.Domain.Enums;

namespace IdentityService.Application.Contracts;

public record LoginResponse(Guid AccountId, string Email, Role Role, string AccessToken);