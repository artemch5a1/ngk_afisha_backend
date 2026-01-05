using IdentityService.Domain.Abstractions.Infrastructure.Repositories.Base;
using IdentityService.Domain.Models.UserContext;

namespace IdentityService.Domain.Abstractions.Infrastructure.Repositories.UserContext;

public interface ISpecialtyRepository : IReadable<Specialty, int>, IWritable<Specialty, int>, IUpdatable<Specialty>, IDeletable<int>;