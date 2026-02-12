using IdentityService.Domain.Abstractions.Infrastructure.Repositories.Base;
using IdentityService.Domain.Models.UserContext;

namespace IdentityService.Domain.Abstractions.Infrastructure.Repositories.UserContext;

public interface IGroupRepository
    : IReadable<Group, int>,
        IWritable<Group, int>,
        IUpdatable<Group>,
        IDeletable<int>
{
    /// <summary>
    /// Получение всех должностей по специальности
    /// </summary>
    /// <param name="specialtyId">id специальности</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Коллекция должностей</returns>
    Task<List<Group>> GetAllBySpecialtyId(
        int specialtyId,
        CancellationToken cancellationToken = default
    );
}
