using IdentityService.Domain.Abstractions.Infrastructure.Repositories.Base;
using IdentityService.Domain.Models.UserContext;

namespace IdentityService.Domain.Abstractions.Infrastructure.Repositories.UserContext;

public interface IUserRepository : IReadable<User, Guid>, IWritable<User, Guid>, IUpdatable<User>
{
    /// <summary>
    /// Обновление вложенного агрегата <see cref="Student"/>
    /// </summary>
    /// <param name="model">Корень агрегата</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Булевое значение(обновлено/не обновлено)</returns>
    Task<bool> UpdateStudentProfile(User model, CancellationToken cancellationToken = default);

    /// <summary>
    /// Обновление вложенного агрегата <see cref="Publisher"/>
    /// </summary>
    /// <param name="model">Корень агрегата</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Булевое значение(обновлено/не обновлено)</returns>
    Task<bool> UpdatePublisherProfile(User model, CancellationToken cancellationToken = default);
}
