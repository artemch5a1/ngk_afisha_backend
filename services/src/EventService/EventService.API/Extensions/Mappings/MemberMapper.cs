using EventService.API.Contracts.Members;
using EventService.Domain.Models;

namespace EventService.API.Extensions.Mappings;

public static class MemberMapper
{
    public static MemberDto ToDto(this Member member)
    {
        MemberDto memberDto = new MemberDto()
        {
            InvitationId = member.InvitationId,
            StudentId = member.StudentId,
            Status = member.Status
        };

        if (member.Invitation is not null)
            memberDto.Invitation = member.Invitation.ToDto();

        return memberDto;
    }

    public static List<MemberDto> ToListDto(this List<Member> members)
        => members.Select(x => x.ToDto()).ToList();
}