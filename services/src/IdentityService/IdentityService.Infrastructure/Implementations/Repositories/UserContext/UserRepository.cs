using IdentityService.Domain.Abstractions.Infrastructure.Mapping;
using IdentityService.Domain.Abstractions.Infrastructure.Repositories.UserContext;
using IdentityService.Domain.Models.UserContext;
using IdentityService.Infrastructure.Data.Database;
using IdentityService.Infrastructure.Entites.UserContext;
using IdentityService.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IdentityService.Infrastructure.Implementations.Repositories.UserContext;

public class UserRepository : IUserRepository
{
    private readonly IdentityServiceDbContext _db;

    private readonly ILogger<UserRepository> _logger;

    private readonly IEntityMapper<UserEntity, User> _userMapper;
    
    public UserRepository(
        IdentityServiceDbContext db, 
        ILogger<UserRepository> logger, 
        IEntityMapper<UserEntity, User> userMapper)
    {
        _db = db;
        _logger = logger;
        _userMapper = userMapper;
    }

    public async Task<List<User>> GetAll(CancellationToken cancellationToken = default)
    {
        try
        {
            List<UserEntity> result = await _db.Users
                .Include(x => x.StudentProfile)
                .Include(x => x.PublisherProfile)
                .ToListAsync(cancellationToken);

            return _userMapper.ToListDomain(result);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "Ошибка при полчении пользователей");
            throw ex.HandleException();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Непредвиденная ошибка при полчении пользователей");
            throw ex.HandleException();
        }
    }

    public async Task<User?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            UserEntity? user = await _db.Users
                .Include(x => x.StudentProfile)
                .Include(x => x.PublisherProfile)
                .FirstOrDefaultAsync(x => x.UserId == id ,cancellationToken);

            if (user is null)
                return null;
            
            return _userMapper.ToDomain(user);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "Ошибка при полчении пользователя по id");
            throw ex.HandleException();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Непредвиденная ошибка при полчении пользователя по id");
            throw ex.HandleException();
        }
    }

    public async Task<User?> FindAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            UserEntity? user = await _db.Users
                .FindAsync(id ,cancellationToken);

            if (user is null)
                return null;
            
            return _userMapper.ToDomain(user);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "Ошибка при полчении пользователя по id");
            throw ex.HandleException();
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Непредвиденная ошибка при полчении пользователя по id");
            throw ex.HandleException();
        }
    }

    public async Task<User> Create(User model, CancellationToken cancellationToken = default)
    {
        try
        {
            UserEntity user = _userMapper.ToEntity(model);

            var result = await _db.Users.AddAsync(user, cancellationToken);

            await _db.SaveChangesAsync(cancellationToken);

            return _userMapper.ToDomain(result.Entity);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "Ошибка при сохранении пользователя");
            throw ex.HandleException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Непредвиденная ошибка при сохранении пользователя");
            throw ex.HandleException();
        }
    }

    public async Task<bool> Update(User model, CancellationToken cancellationToken = default)
    {
        try
        {
            int result = await _db.Users
                .Where(x => x.UserId == model.UserId)
                .ExecuteUpdateAsync(
                    x =>
                        x.SetProperty(i => i.Name, i => model.Name)
                            .SetProperty(i => i.Surname, i => model.Surname)
                            .SetProperty(i => i.Patronymic, i => model.Patronymic)
                            .SetProperty(i => i.BirthDate, i => model.BirthDate),
                    cancellationToken
                );

            return result > 0;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "Ошибка при обновлении пользователя");
            throw ex.HandleException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Непредвиденная ошибка при обновлении пользователя");
            throw ex.HandleException();
        }
    }

    public  async Task<bool> UpdateStudentProfile(User model, CancellationToken cancellationToken = default)
    {
        try
        {
            if (model.StudentProfile is null)
                throw new ArgumentException("У пользователя нет профиля студента");

            Student student = model.StudentProfile;
            
            int result = await _db.Students.Where(x => x.StudentId == model.UserId)
                .ExecuteUpdateAsync(
                    x => x
                        .SetProperty(i => i.GroupId, i => student.GroupId),
                    cancellationToken
                    );

            return result > 0;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "Ошибка при обновлении пользователя");
            throw ex.HandleException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Непредвиденная ошибка при обновлении пользователя");
            throw ex.HandleException();
        }
    }
    
    public  async Task<bool> UpdatePublisherProfile(User model, CancellationToken cancellationToken = default)
    {
        try
        {
            if (model.PublisherProfile is null)
                throw new ArgumentException("У пользователя нет профиля публикатора");

            Publisher student = model.PublisherProfile;
            
            int result = await _db.Publishers.Where(x => x.PublisherId == model.UserId)
                .ExecuteUpdateAsync(
                    x => x
                        .SetProperty(i => i.PostId, i => student.PostId),
                    cancellationToken
                );

            return result > 0;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "Ошибка при обновлении пользователя");
            throw ex.HandleException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Непредвиденная ошибка при обновлении пользователя");
            throw ex.HandleException();
        }
    }
}