using EventService.API.Contracts.Invitation;
using EventService.Application.UseCases.InvitationCases.AcceptRequestOnEventInvitation;
using EventService.Application.UseCases.InvitationCases.CancelRequestOnEventInvitation;
using EventService.Application.UseCases.InvitationCases.CreateInvitation;
using EventService.Application.UseCases.InvitationCases.RejectMemberOnEventInvitation;
using EventService.Application.UseCases.InvitationCases.TakeRequestOnEventInvitation;
using EventService.Application.UseCases.InvitationCases.UpdateInvitation;
using EventService.Domain.Models;

namespace EventService.API.Extensions.Mappings;

public static class InvitationMapper
{
    public static InvitationDto ToDto(this Invitation invitation)
    {
        InvitationDto dto = new InvitationDto()
        {
            InvitationId = invitation.InvitationId,
            EventId = invitation.EventId,
            RoleId = invitation.RoleId,
            ShortDescription = invitation.ShortDescription,
            Description = invitation.Description,
            RequiredMember = invitation.RequiredMember,
            AcceptedMember = invitation.AcceptedMember,
            DeadLine = invitation.DeadLine,
            Status = invitation.Status,
        };

        if (invitation.Event is not null)
            dto.Event = invitation.Event.ToDto();

        if (invitation.Role is not null)
            dto.Role = invitation.Role.ToDto();

        return dto;
    }

    public static UpdateInvitationCommand ToCommand(this UpdateInvitationDto dto, Guid currentUser)
    {
        return new UpdateInvitationCommand(dto.EventId, currentUser, dto.InvitationId, dto.RoleId, dto.ShortDescription,
            dto.Description, dto.RequiredMember, dto.DeadLine);
    }

    public static List<InvitationDto> ToListDto(this List<Invitation> invitations)
        => invitations.Select(x => x.ToDto()).ToList();

    public static CreateInvitationCommand ToCommand(this CreateInvitationDto dto, Guid currentUser)
        => new(
            dto.EventId, 
            currentUser, 
            dto.RoleId, 
            dto.ShortDescription, 
            dto.Description, 
            dto.RequiredMember, 
            dto.DeadLine);

    public static TakeRequestCommand ToCommand(this TakeRequestOnInvitationDto dto, Guid currentStudent)
        => new(dto.EventId, dto.InvitationId, currentStudent);
    
    public static CancelRequestCommand ToCommand(this CancelRequestOnInvitationDto dto, Guid currentStudent)
        => new(dto.EventId, dto.InvitationId, currentStudent);

    public static AcceptRequestCommand ToCommand(this AcceptRequestOnInvitationDto dto, Guid currentUser)
        => new(dto.EventId, dto.InvitationId, dto.StudentId, currentUser);
    
    public static RejectMemberCommand ToCommand(this RejectMemberOnInvitationDto dto, Guid currentUser)
        => new(dto.EventId, dto.InvitationId, dto.StudentId, currentUser);
}