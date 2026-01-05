using IdentityService.Domain.CustomExceptions;

namespace IdentityService.Domain.Models.UserContext;

/// <summary>
/// Модель группы
/// </summary>
public class Group
{
    private Group(int course, int numberGroup, int specialtyId)
    {
        Course = course;
        NumberGroup = numberGroup;
        SpecialtyId = specialtyId;
    }


    public int GroupId { get; private set; }

    public int Course { get; private set; }

    public int NumberGroup { get; private set; }
    
    public int SpecialtyId { get; private set;  }

    public Specialty? Specialty { get; private set; } = null;

    /// <summary>
    /// Первая буква специальности
    /// </summary>
    public char LetterSpecialty => Specialty?.SpecialtyTitle.First() ?? ' ';
    
    /// <summary>
    /// Создание группы
    /// </summary>
    /// <param name="course">Курс</param>
    /// <param name="numberGroup">Номер группы</param>
    /// <param name="specialtyId">id специальности</param>
    /// <returns>Модель группы</returns>
    /// <exception cref="DomainException">Выбрасывается в случае провальной валидации</exception>
    public static Group CreateGroup(int course, int numberGroup, int specialtyId)
    {
        if (course < 1 || course > 4)
            throw new DomainException("Курс должен быть в промежутке от 1 до 4");
        
        return new Group(course, numberGroup, specialtyId);
    }

    /// <summary>
    /// Восстановление группы
    /// </summary>
    /// <param name="groupId">id группы</param>
    /// <param name="course">Курс</param>
    /// <param name="numberGroup">Номер группы</param>
    /// <param name="specialtyId">id специальности</param>
    /// <returns>Модель группы</returns>
    internal static Group Restore(int groupId, int course, int numberGroup, int specialtyId)
    {
        Group group = new Group(course, numberGroup, specialtyId)
        {
            GroupId = groupId
        };

        return group;
    }

    public void UpdateGroup(int newCourse, int newNumberGroup, int newSpecialtyId)
    {
        if (newCourse < 1 || newCourse > 4)
            throw new DomainException("Курс должен быть в промежутке от 1 до 4");

        Course = newCourse;
        NumberGroup = newNumberGroup;
        SpecialtyId = newSpecialtyId;
    }

    /// <summary>
    /// Добавление навигационного свойства на специальность
    /// </summary>
    /// <param name="specialty">специальность</param>
    internal void AddSpecialtyNavigation(Specialty specialty)
    {
        Specialty = specialty;
    }

    /// <summary>
    /// Получение уникального название группы: курс + номер + первая буква специальности
    /// </summary>
    /// <returns>Строку с названием</returns>
    public string GetIdentityGroup() =>  
        $"{Course}{NumberGroup}{LetterSpecialty}";
}