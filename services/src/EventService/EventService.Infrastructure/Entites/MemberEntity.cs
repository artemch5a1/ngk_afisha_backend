using System.ComponentModel.DataAnnotations.Schema;
using EventService.Domain.Abstractions.Infrastructure.Entity;
using EventService.Domain.Enums;
using EventService.Domain.Models;

namespace EventService.Infrastructure.Entites;

public class MemberEntity : IEntity<MemberEntity, Member>
{
    [Column("invitation_id")]
    public Guid InvitationId { get; set; }

    [ForeignKey(nameof(InvitationId))]
    public InvitationEntity Invitation { get; set; } = null!;

    [Column("student_id")]
    public Guid StudentId { get; set; }

    [Column("status")]
    public int Status { get; set; }

    internal MemberEntity() { }

    public MemberEntity(Member member)
    {
        InvitationId = member.InvitationId;
        StudentId = member.StudentId;
        Status = (int)member.Status;
    }

    public Member ToDomain()
    {
        return Member.Restore(InvitationId, StudentId, (MemberStatus)Status);
    }

    public static MemberEntity ToEntity(Member domain)
    {
        return new MemberEntity(domain);
    }
}
