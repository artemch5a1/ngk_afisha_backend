using EventService.Domain.CustomExceptions;

namespace EventService.Domain.Models;

/// <summary>
/// Событие (корень агрегата)
/// </summary>
public class Event
{
    private const int TitleMaxLength = 85;

    private const int TitleMinLength = 7;

    private const int ShortDescriptionMaxLength = 255;

    private const int ShortDescriptionMinLength = 15;

    private const int DescriptionMaxLength = 750;

    private const int DescriptionMinLength = 35;

    private static readonly int[] MinAgeRange = [0, 14, 16, 18];

    public Guid EventId { get; private set; }

    public string Title { get; private set; }

    public string ShortDescription { get; private set; }

    public string Description { get; private set; }

    public DateTime DateStart { get; private set; }

    public int LocationId { get; private set; }

    public Location Location { get; private set; } = null!;

    public int GenreId { get; private set; }

    public Genre Genre { get; private set; } = null!;

    public int TypeId { get; private set; }

    public EventType Type { get; private set; } = null!;

    public int MinAge { get; private set; }

    internal Guid Author { get; private set; }

    public string PreviewUrl { get; private set; }

    public string DownloadUrl { get; private set; } = null!;

    public IReadOnlyList<Invitation> Invitations => _invitations;

    private List<Invitation> _invitations = new();

    private Event(
        Guid eventId,
        string title,
        string shortDescription,
        string description,
        DateTime dateStart,
        int locationId,
        int genreId,
        int typeId,
        int minAge,
        Guid author,
        string previewUrl
    )
    {
        EventId = eventId;
        Title = title;
        ShortDescription = shortDescription;
        Description = description;
        DateStart = dateStart;
        LocationId = locationId;
        GenreId = genreId;
        TypeId = typeId;
        MinAge = minAge;
        Author = author;
        PreviewUrl = previewUrl;
    }

    /// <summary>
    /// Создает новое событие с валидацией входных данных.
    /// </summary>
    /// <param name="eventId">Уникальный идентификатор события.</param>
    /// <param name="title">Название события.</param>
    /// <param name="shortDescription">Короткое описание события.</param>
    /// <param name="description">Полное описание события.</param>
    /// <param name="dateStart">Дата начала события.</param>
    /// <param name="locationId">Идентификатор места проведения.</param>
    /// <param name="genreId">Идентификатор жанра.</param>
    /// <param name="typeId">Идентификатор типа.</param>
    /// <param name="minAge">Минимальный возраст участников.</param>
    /// <param name="author">Идентификатор автора события.</param>
    /// <param name="previewUrl">URL предварительного просмотра события.</param>
    /// <returns>Созданное событие.</returns>
    /// <exception cref="DomainException">Если входные данные не проходят валидацию.</exception>
    public static Event Create(
        Guid eventId,
        string title,
        string shortDescription,
        string description,
        DateTime dateStart,
        int locationId,
        int genreId,
        int typeId,
        int minAge,
        Guid author,
        string previewUrl
    )
    {
        ExecuteValidation(title, shortDescription, description, dateStart, minAge, author);

        return new Event(
            eventId,
            title,
            shortDescription,
            description,
            dateStart,
            locationId,
            genreId,
            typeId,
            minAge,
            author,
            previewUrl
        );
    }

    /// <summary>
    /// Восстанавливает событие из базы данных без повторной валидации.
    /// </summary>
    internal static Event Restore(
        Guid eventId,
        string title,
        string shortDescription,
        string description,
        DateTime dateStart,
        int locationId,
        int genreId,
        int typeId,
        int minAge,
        Guid author,
        string previewUrl
    )
    {
        return new Event(
            eventId,
            title,
            shortDescription,
            description,
            dateStart,
            locationId,
            genreId,
            typeId,
            minAge,
            author,
            previewUrl
        );
    }

    private static void ExecuteValidation(
        string title,
        string shortDescription,
        string description,
        DateTime dateStart,
        int minAge,
        Guid authorId
    )
    {
        if (
            string.IsNullOrWhiteSpace(title)
            || title.Length < TitleMinLength
            || title.Length > TitleMaxLength
        )
        {
            throw new DomainException(
                MaxMinMessage("Название события", TitleMinLength, TitleMaxLength)
            );
        }

        if (
            string.IsNullOrWhiteSpace(shortDescription)
            || shortDescription.Length < ShortDescriptionMinLength
            || shortDescription.Length > ShortDescriptionMaxLength
        )
        {
            throw new DomainException(
                MaxMinMessage(
                    "Короткое описание события",
                    ShortDescriptionMinLength,
                    ShortDescriptionMaxLength
                )
            );
        }

        if (
            string.IsNullOrWhiteSpace(description)
            || description.Length < DescriptionMinLength
            || description.Length > DescriptionMaxLength
        )
        {
            throw new DomainException(
                MaxMinMessage("Описание события", DescriptionMinLength, DescriptionMaxLength)
            );
        }

        if (!MinAgeRange.Contains(minAge))
            throw new DomainException(
                "Некорректное ограничение возраста. Может быть только 14+, 16+, 18+"
            );

        if (dateStart.ToUniversalTime() <= DateTime.Now.ToUniversalTime())
            throw new DomainException("Дата события должна быть в будущем");

        if (Guid.Empty == authorId)
        {
            throw new DomainException("Некорректный автор");
        }
    }

    private static string MaxMinMessage(object someObject, int min, int max) =>
        $"{someObject} не должно быть меньше {min} или больше {max} символов";

    /// <summary>
    /// Проверяет, может ли текущий пользователь удалить событие.
    /// </summary>
    /// <param name="currentUser">Идентификатор пользователя.</param>
    /// <returns>True, если пользователь является автором события; иначе False.</returns>
    public bool IsMayDelete(Guid currentUser)
    {
        return currentUser == Author;
    }

    /// <summary>
    /// Обновляет данные события с валидацией и проверкой прав текущего пользователя.
    /// </summary>
    /// <param name="currentUser">Идентификатор пользователя, который выполняет обновление.</param>
    /// <param name="title">Новое название события.</param>
    /// <param name="shortDescription">Новое короткое описание.</param>
    /// <param name="description">Новое полное описание.</param>
    /// <param name="dateStart">Новая дата начала.</param>
    /// <param name="locationId">Новый идентификатор места.</param>
    /// <param name="genreId">Новый идентификатор жанра.</param>
    /// <param name="typeId">Новый идентификатор типа.</param>
    /// <param name="minAge">Новый минимальный возраст участников.</param>
    /// <exception cref="NotFoundException">Если текущий пользователь не является автором.</exception>
    /// <exception cref="DomainException">Если входные данные не проходят валидацию.</exception>
    public void Update(
        Guid currentUser,
        string title,
        string shortDescription,
        string description,
        DateTime dateStart,
        int locationId,
        int genreId,
        int typeId,
        int minAge
    )
    {
        if (Author != currentUser)
            throw new NotFoundException("Событие", EventId);

        ExecuteValidation(title, shortDescription, description, dateStart, minAge, currentUser);

        Title = title;
        ShortDescription = shortDescription;
        Description = description;
        DateStart = dateStart;
        LocationId = locationId;
        GenreId = genreId;
        TypeId = typeId;
        MinAge = minAge;
    }

    /// <summary>
    /// Добавляет новое приглашение на событие.
    /// </summary>
    /// <param name="currentUser">Идентификатор пользователя, добавляющего приглашение.</param>
    /// <param name="roleId">Идентификатор роли для приглашения.</param>
    /// <param name="shortDescription">Короткое описание приглашения.</param>
    /// <param name="description">Подробное описание приглашения.</param>
    /// <param name="requiredMember">Количество необходимых участников.</param>
    /// <param name="deadLine">Крайний срок подачи заявки на приглашение.</param>
    /// <returns>Созданное приглашение.</returns>
    /// <exception cref="NotFoundException">Если пользователь не автор события или приглашение не найдено.</exception>
    /// <exception cref="DomainException">Если крайний срок превышает дату события.</exception>
    public Invitation AddNewInvitation(
        Guid currentUser,
        int roleId,
        string shortDescription,
        string description,
        int requiredMember,
        DateTime deadLine
    )
    {
        if (currentUser != Author)
            throw new NotFoundException("Событие", EventId);

        if (deadLine.ToUniversalTime() > DateStart.ToUniversalTime())
            throw new DomainException("Дэдлайн нельзя установить позже наступления события");

        Invitation invitation = Invitation.Create(
            Guid.NewGuid(),
            EventId,
            roleId,
            shortDescription,
            description,
            requiredMember,
            deadLine
        );

        _invitations.Add(invitation);

        return invitation;
    }

    public void UpdateInvitation(
        Guid currentUser,
        Guid invitationId,
        int roleId,
        string shortDescription,
        string description,
        int requiredMember,
        DateTime deadLine
    )
    {
        if (currentUser != Author)
            throw new NotFoundException("Событие", EventId);

        if (deadLine.ToUniversalTime() > DateStart.ToUniversalTime())
            throw new DomainException("Дэдлайн нельзя установить позже наступления события");

        Invitation? invitation = _invitations.FirstOrDefault(x => x.InvitationId == invitationId);

        if (invitation is null)
            throw new NotFoundException("Приглашение", invitationId);

        invitation.Update(shortDescription, description, requiredMember, deadLine, roleId);
    }

    /// <summary>
    /// Удаляет приглашение по идентификатору.
    /// </summary>
    public void RemoveInvitation(Guid currentUser, Guid invitationId)
    {
        if (currentUser != Author)
            throw new NotFoundException("Событие", EventId);

        Invitation? invitation = _invitations.FirstOrDefault(x => x.InvitationId == invitationId);

        if (invitation is null)
            throw new NotFoundException("Приглашение", invitationId);

        _invitations.Remove(invitation);
    }

    /// <summary>
    /// Отмечает заявку на приглашение от студента.
    /// </summary>
    /// <param name="studentId">Студент</param>
    /// <param name="invitationId">Приглашение</param>
    /// <exception cref="NotFoundException">Приглашение не найдено</exception>
    public void TakeRequestByInvitationId(Guid studentId, Guid invitationId)
    {
        Invitation? invitation = _invitations.FirstOrDefault(x => x.InvitationId == invitationId);

        if (invitation is null)
            throw new NotFoundException("Приглашение", invitationId);

        invitation.TakeRequest(studentId);
    }

    /// <summary>
    /// Отменяет заявку на приглашение от студента.
    /// </summary>
    /// <param name="studentId">Студент</param>
    /// <param name="invitationId">Приглашение</param>
    /// <exception cref="NotFoundException">Приглашение не найдено</exception>
    public void CancelRequestByInvitationId(Guid studentId, Guid invitationId)
    {
        Invitation? invitation = _invitations.FirstOrDefault(x => x.InvitationId == invitationId);

        if (invitation is null)
            throw new NotFoundException("Приглашение", invitationId);

        invitation.CancelRequest(studentId);
    }

    /// <summary>
    /// Принимает заявку студента на приглашение.
    /// </summary>
    /// <param name="studentId">Студент</param>
    /// <param name="invitationId">Приглашение</param>
    /// <param name="currentUser">Текущий пользователь, совершающий операцию</param>
    /// <exception cref="NotFoundException">Приглашение не найдено</exception>
    public void AcceptRequestByInvitationId(Guid studentId, Guid invitationId, Guid currentUser)
    {
        if (currentUser != Author)
            throw new NotFoundException("Приглашение", invitationId);

        Invitation? invitation = _invitations.FirstOrDefault(x => x.InvitationId == invitationId);

        if (invitation is null)
            throw new NotFoundException("Приглашение", invitationId);

        invitation.AcceptRequest(studentId);
    }

    /// <summary>
    /// Удаляет студента из участников.
    /// </summary>
    /// <param name="studentId">Студент</param>
    /// <param name="invitationId">Приглашение</param>
    /// <param name="currentUser">Текущий пользователь, совершающий операцию</param>
    /// <exception cref="NotFoundException">Приглашение не найдено</exception>
    public void RejectMemberByInvitationId(Guid studentId, Guid invitationId, Guid currentUser)
    {
        if (currentUser != Author)
            throw new NotFoundException("Приглашение", invitationId);

        Invitation? invitation = _invitations.FirstOrDefault(x => x.InvitationId == invitationId);

        if (invitation is null)
            throw new NotFoundException("Приглашение", invitationId);

        invitation.RejectMember(studentId);
    }

    public void SetDownloadUrl(string downloadUrl)
    {
        DownloadUrl = downloadUrl;
    }

    internal void AddLocationNavigation(Location location)
    {
        Location = location;
    }

    internal void AddGenreNavigation(Genre genre)
    {
        Genre = genre;
    }

    internal void AddInvitationNavigation(List<Invitation> invitations)
    {
        _invitations = invitations;
    }

    internal void AddEventTypeNavigation(EventType eventType)
    {
        Type = eventType;
    }
}
