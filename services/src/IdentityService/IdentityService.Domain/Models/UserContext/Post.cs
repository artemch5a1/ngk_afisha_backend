using IdentityService.Domain.CustomExceptions;

namespace IdentityService.Domain.Models.UserContext;

/// <summary>
/// Модель должности
/// </summary>
public class Post
{
    private const int MaxTitleLength = 200;
    
    public int PostId { get; private set; }
    
    public string Title { get; private set; }
    
    public int DepartmentId { get; private set; }
    
    public Department Department { get; private set; } = null!;
    
    private Post(string title, int departmentId)
    {
        Title = title;
        DepartmentId = departmentId;
    }

    /// <summary>
    /// Метод на создание должности
    /// </summary>
    /// <param name="title">Название должности</param>
    /// <param name="departmentId">Id отдела</param>
    /// <returns>Модель должности</returns>
    /// <exception cref="DomainException">Выбрасывается при провальной валидации</exception>
    public static Post Create(string title, int departmentId)
    {
        if (string.IsNullOrEmpty(title))
            throw new DomainException("Название специальности не может быть пустым");
        
        if (title.Length > MaxTitleLength)
            throw new DomainException($"Название отдела не может превышать {MaxTitleLength} символов");

        return new Post(title, departmentId);
    }

    /// <summary>
    /// Метод на восстановление должности из базы данных
    /// </summary>
    /// <param name="postId">Id должности</param>
    /// <param name="title">Название должности</param>
    /// <param name="departmentId">Id отдела</param>
    /// <returns>Модель должности</returns>
    internal static Post Restore(int postId, string title, int departmentId)
    {
        return new Post(title, departmentId) { PostId = postId };
    }


    /// <summary>
    /// Обновление должности
    /// </summary>
    /// <param name="title">Новое название должности</param>
    /// <param name="departmentId">Новый отдел должности</param>
    /// <exception cref="DomainException">Выбрасывается при провальной валидации</exception>
    public void UpdatePost(string title, int departmentId)
    {
        if (string.IsNullOrEmpty(title))
            throw new DomainException("Название специальности не может быть пустым");
        
        if (title.Length > MaxTitleLength)
            throw new DomainException($"Название отдела не может превышать {MaxTitleLength} символов");

        Title = title;
        DepartmentId = departmentId;
    }

    /// <summary>
    /// Добавление навигиационного свойства отдел
    /// </summary>
    /// <param name="department">Отдел</param>
    internal void AddDepartmentNavigation(Department department)
    {
        Department = department;
    }
}