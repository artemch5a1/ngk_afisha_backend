using IdentityService.Domain.Abstractions.Infrastructure.Repositories.Base;
using IdentityService.Domain.Models.UserContext;

namespace IdentityService.Domain.Abstractions.Infrastructure.Repositories.UserContext;

public interface IDepartmentRepository
    : IReadable<Department, int>,
        IWritable<Department, int>,
        IUpdatable<Department>,
        IDeletable<int>;
