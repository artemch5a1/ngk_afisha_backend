using System.ComponentModel.DataAnnotations.Schema;
using EventService.Domain.Abstractions.Infrastructure.Entity;
using EventService.Domain.Enums;
using EventService.Domain.Models;

namespace EventService.Infrastructure.Entites;

public class InvitationEntity : IEntity<InvitationEntity, Invitation>
{
    [Column("invitation_id")]
    public Guid InvitationId { get; set; }

    [Column("event_id")]
    public Guid EventId { get; set; }

    [ForeignKey(nameof(EventId))]
    public EventEntity Event { get; set; } = null!;

    [Column("role_id")]
    public int RoleId { get; set; }

    [ForeignKey(nameof(RoleId))]
    public EventRoleEntity Role { get; set; } = null!;

    [Column("short_description")]
    public string ShortDescription { get; set; } = null!;

    [Column("description")]
    public string Description { get; set; } = null!;

    [Column("required_member")]
    public int RequiredMember { get; set; }

    [Column("accepted_member")]
    public int AcceptedMember { get; set; } = 0;

    [Column("dead_line")]
    public DateTime DeadLine { get; set; }

    [Column("status")]
    public int Status { get; set; }

    public ICollection<MemberEntity> Members { get; set; } = new List<MemberEntity>();

    internal InvitationEntity() { }

    private InvitationEntity(Invitation invitation)
    {
        InvitationId = invitation.InvitationId;
        EventId = invitation.EventId;
        RoleId = invitation.RoleId;
        ShortDescription = invitation.ShortDescription;
        Description = invitation.Description;
        RequiredMember = invitation.RequiredMember;
        AcceptedMember = invitation.AcceptedMember;
        DeadLine = invitation.DeadLine;
        Status = (int)invitation.Status;

        Members = invitation.Members.Select(MemberEntity.ToEntity).ToList();
    }

    public Invitation ToDomain()
    {
        return Invitation.Restore(
            InvitationId,
            EventId,
            RoleId,
            ShortDescription,
            Description,
            RequiredMember,
            AcceptedMember,
            DeadLine,
            (InvitationStatus)Status
        );
    }

    public static InvitationEntity ToEntity(Invitation domain)
    {
        return new InvitationEntity(domain);
    }
}
