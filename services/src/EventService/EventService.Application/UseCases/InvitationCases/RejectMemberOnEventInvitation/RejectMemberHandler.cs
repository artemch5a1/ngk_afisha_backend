using EventService.Domain.Abstractions.Application.Services.AppServices;
using EventService.Domain.Enums;
using EventService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventService.Application.UseCases.InvitationCases.RejectMemberOnEventInvitation;

public class RejectMemberHandler: IRequestHandler<RejectMemberCommand, Result<Guid>>
{
    private readonly IEventService _eventService;

    private readonly ILogger<RejectMemberHandler> _logger;


    public RejectMemberHandler(
        IEventService eventService, 
        ILogger<RejectMemberHandler> logger)
    {
        _eventService = eventService;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(RejectMemberCommand request, CancellationToken cancellationToken)
    {
        try
        {
            bool result = await _eventService.RejectMemberOnInvitation(
                request.EventId,
                request.InvitationId,
                request.StudentId,
                request.CurrentUser,
                cancellationToken
            );
            
            return result
                ? Result<Guid>.Success(request.InvitationId)
                : Result<Guid>.Failure(["Ошибка при удалении участника"], ApiErrorType.BadRequest);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка при удалении участника");
            
            return Result<Guid>.Failure(ex);
        }
    }
}