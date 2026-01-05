using IdentityService.Domain.Abstractions.Infrastructure.Repositories.Base;
using IdentityService.Domain.Models.UserContext;

namespace IdentityService.Domain.Abstractions.Infrastructure.Repositories.UserContext;

public interface IPostRepository :
    IReadable<Post, int>,
    IWritable<Post, int>,
    IUpdatable<Post>,
    IDeletable<int>
{
    /// <summary>
    /// Получение всех должностей в отделе
    /// </summary>
    /// <param name="departmentId">id отдела</param>
    /// <param name="cancellationToken">Токен отмены</param>
    /// <returns>Коллекция должностей</returns>
    Task<List<Post>> GetAllPostByDepartmentId(int departmentId, CancellationToken cancellationToken = default);
}