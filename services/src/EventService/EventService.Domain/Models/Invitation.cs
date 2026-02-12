using EventService.Domain.CustomExceptions;
using EventService.Domain.Enums;

namespace EventService.Domain.Models;

/// <summary>
/// Представляет приглашение на участие в мероприятии.
/// </summary>
public class Invitation
{
    private const int ShortDescriptionMaxLength = 255;

    private const int ShortDescriptionMinLength = 15;

    private const int DescriptionMaxLength = 750;

    private const int DescriptionMinLength = 35;

    public Guid InvitationId { get; private set; }

    public Guid EventId { get; private set; }

    public Event Event { get; private set; } = null!;

    public int RoleId { get; private set; }

    public EventRole Role { get; private set; } = null!;

    public string ShortDescription { get; private set; }

    public string Description { get; private set; }

    public int RequiredMember { get; private set; }

    public int AcceptedMember { get; private set; } = 0;

    public DateTime DeadLine { get; private set; }

    public InvitationStatus Status { get; private set; }

    public IReadOnlyList<Member> Members => _members;

    private List<Member> _members = new();

    private Invitation(
        Guid invitationId,
        Guid eventId,
        int roleId,
        string shortDescription,
        string description,
        int requiredMember,
        DateTime deadLine
    )
    {
        InvitationId = invitationId;
        EventId = eventId;
        RoleId = roleId;
        ShortDescription = shortDescription;
        Description = description;
        RequiredMember = requiredMember;
        DeadLine = deadLine;
        Status = InvitationStatus.Active;
    }

    internal static Invitation Create(
        Guid invitationId,
        Guid eventId,
        int roleId,
        string shortDescription,
        string description,
        int requiredMember,
        DateTime deadLine
    )
    {
        ExecuteValidation(shortDescription, description, requiredMember);

        return new Invitation(
            invitationId,
            eventId,
            roleId,
            shortDescription,
            description,
            requiredMember,
            deadLine
        );
    }

    internal void Update(
        string shortDescription,
        string description,
        int requiredMember,
        DateTime deadLine,
        int? roleId = null
    )
    {
        ExecuteValidation(shortDescription, description, requiredMember);

        if (_members.Count > requiredMember)
            throw new DomainException(
                "Новое количество требуемых участников меньше, чем количество уже набраных"
            );

        Description = description;
        ShortDescription = shortDescription;
        RequiredMember = requiredMember;
        DeadLine = deadLine;

        Status = AcceptedMember >= RequiredMember ? InvitationStatus.Done : InvitationStatus.Active;

        if (roleId is not null)
            RoleId = (int)roleId;
    }

    internal static Invitation Restore(
        Guid invitationId,
        Guid eventId,
        int roleId,
        string shortDescription,
        string description,
        int requiredMember,
        int acceptedMember,
        DateTime deadLine,
        InvitationStatus status
    )
    {
        return new Invitation(
            invitationId,
            eventId,
            roleId,
            shortDescription,
            description,
            requiredMember,
            deadLine
        )
        {
            AcceptedMember = acceptedMember,
            Status = status,
        };
    }

    private static void ExecuteValidation(
        string shortDescription,
        string description,
        int requiredMember
    )
    {
        if (requiredMember < 1)
            throw new DomainException("Количество требуемых участников должно быть больше 0");

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
    }

    private static string MaxMinMessage(object someObject, int min, int max) =>
        $"{someObject} не должно быть меньше {min} или больше {max} символов";

    internal void AddMemberNavigation(List<Member> members)
    {
        _members = members;
    }

    internal void AddEventNavigation(Event @event)
    {
        Event = @event;
    }

    internal void AddRoleNavigation(EventRole role)
    {
        Role = role;
    }

    /// <summary>
    /// Подаёт заявку студента на участие в приглашении.
    /// </summary>
    /// <param name="studentId">Идентификатор студента, подающего заявку.</param>
    /// <exception cref="DomainException">
    /// Выбрасывается, если приглашение не активно, студент уже подал заявку,
    /// срок подачи истёк или достигнут лимит участников.
    /// </exception>
    internal void TakeRequest(Guid studentId)
    {
        if (Status != InvitationStatus.Active)
            throw new DomainException("Нельзя подать заявку: приглашение не активно.");

        if (_members.Any(x => x.StudentId == studentId))
            throw new DomainException("Студент уже подал заявку");

        if (DeadLine.ToUniversalTime() < DateTime.Now.ToUniversalTime())
            throw new DomainException("Время для подачи заявок окончено");

        if (AcceptedMember >= RequiredMember)
            throw new DomainException("Необходимое количество участников уже набрано");

        Member member = Member.Create(InvitationId, studentId);

        _members.Add(member);
    }

    /// <summary>
    /// Отменяет заявку студента на участие в приглашении, если она еще не принята.
    /// </summary>
    /// <param name="studentId">Идентификатор студента, подающего заявку.</param>
    /// <exception cref="DomainException">
    /// Выбрасывается, если студент не является учатником или его заявка уже принята
    /// </exception>
    internal void CancelRequest(Guid studentId)
    {
        Member? member = _members.FirstOrDefault(x =>
            x.StudentId == studentId && x.Status != MemberStatus.Accepted
        );

        if (member is null)
            throw new DomainException("Заявки не сущесвтует");

        _members.Remove(member);
    }

    /// <summary>
    /// Одобряет заявку указанного студента на участие в мероприятии.
    /// </summary>
    /// <param name="studentId">Идентификатор студента, чья заявка одобряется.</param>
    /// <exception cref="DomainException">
    /// Выбрасывается, если заявка не найдена, уже одобрена
    /// или достигнут лимит участников.
    /// </exception>
    internal void AcceptRequest(Guid studentId)
    {
        var member =
            _members.FirstOrDefault(x => x.StudentId == studentId)
            ?? throw new DomainException("Заявка не найдена.");

        if (member.Status == MemberStatus.Accepted)
            throw new DomainException("Заявка уже одобрена.");

        if (AcceptedMember >= RequiredMember)
            throw new DomainException("Нельзя одобрить заявку: лимит участников достигнут.");

        member.ChangeStatus(MemberStatus.Accepted);
        AcceptedMember = _members.Count(m => m.Status == MemberStatus.Accepted);

        if (AcceptedMember >= RequiredMember)
            Status = InvitationStatus.Done;
    }

    internal void RejectMember(Guid studentId)
    {
        var member =
            _members.FirstOrDefault(x => x.StudentId == studentId)
            ?? throw new DomainException("Заявка не найдена.");

        _members.Remove(member);
        AcceptedMember = _members.Count(m => m.Status == MemberStatus.Accepted);

        if (AcceptedMember < RequiredMember)
            Status = InvitationStatus.Active;
    }
}
