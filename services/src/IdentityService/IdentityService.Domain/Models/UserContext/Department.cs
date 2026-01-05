using IdentityService.Domain.CustomExceptions;

namespace IdentityService.Domain.Models.UserContext;

/// <summary>
/// Модель отдела
/// </summary>
public class Department
{
    private const int MaxTitleLength = 200;
    
    public int DepartmentId { get; private set; }

    public string Title { get; private set; }

    private Department(string title)
    {
        Title = title;
    }

    /// <summary>
    /// Создание нового отдела
    /// </summary>
    /// <param name="title">Название отдела</param>
    /// <returns>Новую модель отдела</returns>
    /// <exception cref="DomainException">Выбрасывается при проавле валидации</exception>
    public static Department CreateDepartment(string title)
    {
        if (string.IsNullOrEmpty(title))
            throw new DomainException("Название специальности не может быть пустым");
        
        if (title.Length > MaxTitleLength)
            throw new DomainException($"Название отдела не может превышать {MaxTitleLength} символов");
        
        return new Department(title);
    }

    /// <summary>
    /// Восстановление отдела из базы данных
    /// </summary>
    /// <param name="departmentId">id отдела</param>
    /// <param name="title">название отдела</param>
    /// <returns>Модель отдела</returns>
    internal static Department Restore(int departmentId, string title)
    {
        return new Department(title){ DepartmentId = departmentId };
    }

    /// <summary>
    /// Обновление отдела
    /// </summary>
    /// <param name="title">Новое название отдела</param>
    /// <exception cref="DomainException">Выбрасывается при проавле валидации</exception>
    public void UpdateDepartment(string title)
    {
        if (string.IsNullOrEmpty(title))
            throw new DomainException("Название специальности не может быть пустым");
        
        if (title.Length > MaxTitleLength)
            throw new DomainException($"Название отдела не может превышать {MaxTitleLength} символов");

        Title = title;
    }
}