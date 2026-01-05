namespace IdentityService.Domain.Models.UserContext;

/// <summary>
/// Модель профиля студента
/// </summary>
public class Student
{
    private Student(Guid studentId, int groupId)
    {
        StudentId = studentId;
        GroupId = groupId;
    }

    public Guid StudentId { get; private set; }

    public User? User { get; private set; } = null;

    public int GroupId { get; private set; }

    public Group? Group { get; private set; } = null;
    
    /// <summary>
    /// Создание профиля студента
    /// </summary>
    /// <param name="studentId">Id студента</param>
    /// <param name="groupId">Id группы студента</param>
    /// <returns>Модель студента</returns>
    internal static Student Create(Guid studentId, int groupId)
    {
        return new Student(studentId, groupId);
    }

    /// <summary>
    /// Восстановаление студента из репозитория
    /// </summary>
    /// <param name="studentId">Id студента</param>
    /// <param name="groupId">Id группы студента</param>
    /// <returns>Модель студента</returns>
    internal static Student Restore(Guid studentId, int groupId)
    {
        return new Student(studentId, groupId);
    }
    
    /// <summary>
    /// Смена группы студента
    /// </summary>
    /// <param name="newGroup">Id новой группы</param>
    internal void UpdateStudent(int newGroup)
    {
        GroupId = newGroup;
    }

    /// <summary>
    /// Добавление навигационного свойства на группу студента
    /// </summary>
    /// <param name="group">Группа студента</param>
    internal void AddGroupNavigation(Group group)
    {
        Group = group;
    }

    /// <summary>
    /// Добавление навигационного свойства на пользователя, владеющего
    /// профилем студента
    /// </summary>
    /// <param name="user">Пользователь</param>
    internal void AddUserNavigation(User user)
    {
        User = user;
    }
}