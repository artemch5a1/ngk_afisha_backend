using EventService.Domain.Enums;

namespace EventService.Domain.Models;

/// <summary>
/// Представляет участника приглашения на мероприятие.
/// </summary>
public class Member
{
    public Guid InvitationId { get; private set; }

    public Invitation Invitation { get; private set; } = null!;

    public Guid StudentId { get; private set; }

    public MemberStatus Status { get; private set; }

    private Member(Guid invitationId, Guid studentId, MemberStatus status)
    {
        InvitationId = invitationId;
        StudentId = studentId;
        Status = status;
    }

    /// <summary>
    /// Создаёт нового участника для указанного приглашения.
    /// </summary>
    /// <param name="invitationId">Идентификатор приглашения.</param>
    /// <param name="studentId">Идентификатор студента.</param>
    /// <returns>Новый экземпляр <see cref="Member"/> со статусом Review.</returns>
    internal static Member Create(Guid invitationId, Guid studentId)
    {
        return new Member(invitationId, studentId, MemberStatus.Review);
    }

    /// <summary>
    /// Восстанавливает существующего участника (например, из базы данных).
    /// </summary>
    /// <param name="invitationId">Идентификатор приглашения.</param>
    /// <param name="studentId">Идентификатор студента.</param>
    /// <param name="status">Статус участника.</param>
    /// <returns>Восстановленный экземпляр <see cref="Member"/>.</returns>
    internal static Member Restore(Guid invitationId, Guid studentId, MemberStatus status)
    {
        return new Member(invitationId, studentId, status);
    }

    /// <summary>
    /// Изменяет статус участника.
    /// </summary>
    /// <param name="newStatus">Новый статус участника.</param>
    internal void ChangeStatus(MemberStatus newStatus)
    {
        Status = newStatus;
    }

    /// <summary>
    /// Добавляет навигационное свойство к приглашению.
    /// </summary>
    /// <param name="invitation">Приглашение, к которому относится участник.</param>
    internal void AddInvitationNavigation(Invitation invitation)
    {
        Invitation = invitation;
    }
}
