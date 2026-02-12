using IdentityService.Domain.CustomExceptions;

namespace IdentityService.Domain.Models.UserContext;

/// <summary>
/// Модель пользователя системы
/// </summary>
public class User
{
    private const int MaxSurnameCount = 255;

    private const int MaxPatronymicCount = 255;

    private const int MaxNameCount = 255;

    private const int MinSurnameCount = 1;

    private const int MinPatronymicCount = 1;

    private const int MinNameCount = 1;

    private const int MinAge = 14;

    private User(Guid userId, string surname, string name, string? patronymic, DateOnly birthDate)
    {
        Surname = surname;
        Name = name;
        Patronymic = patronymic;
        BirthDate = birthDate;
        UserId = userId;
    }

    public Guid UserId { get; private set; }

    public string Surname { get; private set; }

    public string Name { get; private set; }

    public string? Patronymic { get; private set; }

    public DateOnly BirthDate { get; private set; }

    public Student? StudentProfile { get; private set; }

    public Publisher? PublisherProfile { get; private set; }

    /// <summary>
    /// Метод для создания нового пользователя (студента) (инкапсулирует в себе валидацию)
    /// </summary>
    /// <param name="userId">идентификатор пользователя в uuid (создается на этапе бизнес-логики)</param>
    /// <param name="surname">фамилия</param>
    /// <param name="name">имя</param>
    /// <param name="patronymic">отчество (может быть null). Если придет пустая строка, она преобразуется в null</param>
    /// <param name="birthDate">Дата рождения.</param>
    /// <param name="groupId">Id группы студента.</param>
    /// <returns>Возвращает созданный экземпляр пользователя</returns>
    /// <exception cref="DomainException">Выбрасывается при попытке создать
    /// невалидную модель</exception>
    public static User CreateStudent(
        Guid userId,
        string surname,
        string name,
        string? patronymic,
        DateOnly birthDate,
        int groupId
    )
    {
        User student = CreateUser(userId, surname, name, patronymic, birthDate);

        Student studentProfile = Student.Create(userId, groupId);

        student.StudentProfile = studentProfile;

        return student;
    }

    /// <summary>
    /// Метод для создания нового пользователя (публикатора) (инкапсулирует в себе валидацию)
    /// </summary>
    /// <param name="userId">идентификатор пользователя в uuid (создается на этапе бизнес-логики)</param>
    /// <param name="surname">фамилия</param>
    /// <param name="name">имя</param>
    /// <param name="patronymic">отчество (может быть null). Если придет пустая строка, она преобразуется в null</param>
    /// <param name="birthDate">Дата рождения.</param>
    /// <param name="postId">Id должности публикатора.</param>
    /// <returns>Возвращает созданный экземпляр пользователя</returns>
    /// <exception cref="DomainException">Выбрасывается при попытке создать
    /// невалидную модель</exception>
    public static User CreatePublisher(
        Guid userId,
        string surname,
        string name,
        string? patronymic,
        DateOnly birthDate,
        int postId
    )
    {
        User publisher = CreateUser(userId, surname, name, patronymic, birthDate);

        Publisher publisherProfile = Publisher.Create(userId, postId);

        publisher.PublisherProfile = publisherProfile;

        return publisher;
    }

    private static User CreateUser(
        Guid userId,
        string surname,
        string name,
        string? patronymic,
        DateOnly birthDate
    )
    {
        if (string.IsNullOrWhiteSpace(patronymic))
            patronymic = null;

        ExecuteValidation(surname, name, patronymic, birthDate);

        return new User(userId, surname, name, patronymic, birthDate);
    }

    /// <summary>
    /// Закрытый в сборке метод для воссоздания существующего объекта.
    /// </summary>
    /// <param name="userId">идентификатор пользователя</param>
    /// <param name="surname">фамилия</param>
    /// <param name="name">имя</param>
    /// <param name="patronymic">отчество</param>
    /// <param name="birthDate">дата рождения</param>
    /// <param name="studentProfile">Профиль студента (если это студент)</param>
    /// <remarks>
    /// <list type="number">
    /// <item>
    /// Доступен слою Infrastructure. Валидация не происходит.
    /// </item>
    /// <item>
    /// Проверяется только критические бизнес варианты (наличие идентификатора, уникальных индексов и т.д.)
    /// </item>
    /// </list>
    /// </remarks>
    /// <returns>Возвращает созданный экземпляр пользователя</returns>
    /// <exception cref="InvalidDataException">Выбрасывается при некорректности критических
    /// полей. Ответсвтенность лежит на Infrastructure</exception>
    internal static User Restore(
        Guid userId,
        string surname,
        string name,
        string? patronymic,
        DateOnly birthDate
    )
    {
        if (userId == Guid.Empty)
            throw new InvalidDataException("AccountId не может быть пустым");

        return new User(userId, surname, name, patronymic, birthDate);
    }

    /// <summary>
    /// Валидация. Бизнес правила модели.
    /// </summary>
    /// <param name="surname">Фамилия</param>
    /// <param name="name">Имя</param>
    /// <param name="patronymic">Отчество</param>
    /// <param name="birthDate">Дата рождения</param>
    /// <exception cref="DomainException">Выбрасывается при провальной валидации</exception>
    private static void ExecuteValidation(
        string surname,
        string name,
        string? patronymic,
        DateOnly birthDate
    )
    {
        if (
            string.IsNullOrWhiteSpace(surname)
            || surname.Length < MinSurnameCount
            || surname.Length > MaxSurnameCount
        )
            throw new DomainException(
                $"Фамилия должна содержать больше {MinSurnameCount} и меньше {MaxSurnameCount} символов"
            );

        if (
            string.IsNullOrWhiteSpace(name)
            || name.Length < MinNameCount
            || name.Length > MaxNameCount
        )
            throw new DomainException(
                $"Имя должно содержать больше {MinNameCount} и меньше {MaxNameCount} символов"
            );

        if (
            patronymic is not null
            && (patronymic.Length < MinPatronymicCount || patronymic.Length > MaxPatronymicCount)
        )
            throw new DomainException(
                $"Отчество должно содержать больше {MinPatronymicCount} и меньше {MaxPatronymicCount} символов"
            );

        int age = CalculateAge(birthDate);

        if (age > 99)
            throw new DomainException($"Некорректная дата рождения");

        if (age < MinAge)
            throw new DomainException($"Пользователь должен быть не младше {MinAge} лет");
    }

    /// <summary>
    /// Вычисление возраста по дате рождения.
    /// </summary>
    /// <param name="birthDate">Дата рождения</param>
    /// <remarks>
    /// Расчёт возраста:
    /// <list type="number">
    ///   <item>
    ///     <description>Определяется разница в годах между текущей датой и датой рождения.</description>
    ///   </item>
    ///   <item>
    ///     <description>Если день рождения ещё не наступил в текущем году (включая сам день рождения),
    ///     возраст уменьшается на 1.</description>
    ///   </item>
    ///   <item>
    ///     <description>Таким образом, возраст считается достигнутым только на следующий день после даты рождения
    ///     (норма ГК РФ, ст. 191).</description>
    ///   </item>
    /// </list>
    /// </remarks>
    /// <returns>Возвращает количество полных лет</returns>
    public static int CalculateAge(DateOnly birthDate)
    {
        var today = DateTime.UtcNow;
        var age = today.Year - birthDate.Year;

        if (
            today.Month < birthDate.Month
            || (today.Month == birthDate.Month && today.Day <= birthDate.Day)
        )
        {
            age--;
        }

        return age;
    }

    /// <summary>
    /// Обноваление профиля студента
    /// </summary>
    /// <param name="newGroupId">Id новой группы</param>
    /// <exception cref="DomainException">Выбрасывается, если у пользователя нет профиля студента</exception>
    public void UpdateStudentProfile(int newGroupId)
    {
        if (StudentProfile is null)
            throw new DomainException("У пользователя нет студенческого профиля");

        StudentProfile.UpdateStudent(newGroupId);
    }

    /// <summary>
    /// Обноваление профиля публикатора
    /// </summary>
    /// <param name="newPostId">Id новой должности</param>
    /// <exception cref="DomainException">Выбрасывается, если у пользователя нет профиля публикатора</exception>
    public void UpdatePublisherProfile(int newPostId)
    {
        if (PublisherProfile is null)
            throw new DomainException("У пользователя нет профиля публикатора");

        PublisherProfile.UpdatePublisher(newPostId);
    }

    /// <summary>
    /// Добавление навигационного свойства на профиль студента
    /// </summary>
    /// <param name="student">Профиль студента</param>
    internal void AddStudentProfile(Student student)
    {
        StudentProfile = student;
    }

    /// <summary>
    /// Добавление навигационного свойства на профиль публикатора
    /// </summary>
    /// <param name="publisher">Профиль публикатора</param>
    internal void AddPublisherProfile(Publisher publisher)
    {
        PublisherProfile = publisher;
    }

    /// <summary>
    /// Функция для обновления всех полей пользователя.
    /// Инкапсулирует в себе валидацию
    /// </summary>
    /// <param name="surname">Фамилия</param>
    /// <param name="name">Имя</param>
    /// <param name="patronymic">Отчество</param>
    /// <param name="birthDate">Дата рождения</param>
    public void UpdateFields(string surname, string name, string? patronymic, DateOnly birthDate)
    {
        if (string.IsNullOrEmpty(patronymic))
            patronymic = null;

        ExecuteValidation(surname, name, patronymic, birthDate);

        Surname = surname;
        Name = name;
        Patronymic = patronymic;
        BirthDate = birthDate;
    }
}
