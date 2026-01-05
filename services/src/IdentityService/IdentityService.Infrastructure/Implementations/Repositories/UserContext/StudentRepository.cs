using IdentityService.Domain.Abstractions.Infrastructure.Mapping;
using IdentityService.Domain.Abstractions.Infrastructure.Repositories.UserContext;
using IdentityService.Domain.Models.UserContext;
using IdentityService.Infrastructure.Data.Database;
using IdentityService.Infrastructure.Entites.UserContext;
using IdentityService.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace IdentityService.Infrastructure.Implementations.Repositories.UserContext;

public class StudentRepository : IStudentRepository
{
    private readonly IdentityServiceDbContext _db;

    private readonly ILogger<StudentRepository> _logger;

    private readonly IEntityMapper<StudentEntity, Student> _studentMapper;


    public StudentRepository(
        IdentityServiceDbContext db, 
        ILogger<StudentRepository> logger, 
        IEntityMapper<StudentEntity, Student> studentMapper)
    {
        _db = db;
        _logger = logger;
        _studentMapper = studentMapper;
    }

    public async Task<List<Student>> GetAll(CancellationToken cancellationToken = default)
    {
        try
        {
            List<StudentEntity> students = await _db.Students
                .Include(x => x.User)
                .Include(x => x.Group)
                .ThenInclude(x => x.Specialty)
                .ToListAsync(cancellationToken);

            return _studentMapper.ToListDomain(students);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "Ошибка при получении студентов");
            throw ex.HandleException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Непредвиденная ошибка при получении студентов");
            throw ex.HandleException();
        }
    }

    public async Task<Student?> GetById(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            StudentEntity? student = await _db.Students
                .Include(x => x.User)
                .Include(x => x.Group)
                .ThenInclude(x => x.Specialty)
                .FirstOrDefaultAsync(x => x.StudentId == id, cancellationToken);

            if (student is null)
                return null;
            
            return _studentMapper.ToDomain(student);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "Ошибка при получении студента");
            throw ex.HandleException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Непредвиденная ошибка при получении студента");
            throw ex.HandleException();
        }
    }

    public async Task<Student?> FindAsync(Guid id, CancellationToken cancellationToken = default)
    {
        try
        {
            StudentEntity? student = await _db.Students
                .FindAsync(id, cancellationToken);

            if (student is null)
                return null;
            
            return _studentMapper.ToDomain(student);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogWarning(ex, "Ошибка при получении студента");
            throw ex.HandleException();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Непредвиденная ошибка при получении студента");
            throw ex.HandleException();
        }
    }
}