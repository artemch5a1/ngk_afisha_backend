using EventService.Domain.Abstractions.Application.Services.AppServices;
using EventService.Domain.Enums;
using EventService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventService.Application.UseCases.InvitationCases.DeleteInvitation;

public class DeleteInvitationHandler : IRequestHandler<DeleteInvitationCommand, Result<Guid>>
{
    private readonly IEventService _eventService;

    private readonly ILogger<DeleteInvitationHandler> _logger;

    public DeleteInvitationHandler(
        IEventService eventService, 
        ILogger<DeleteInvitationHandler> logger)
    {
        _eventService = eventService;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(DeleteInvitationCommand request, CancellationToken cancellationToken)
    {
        try
        {
            bool result = await _eventService.DeleteInvitation(
                request.EventId, 
                request.InvitationId,
                request.CurrentUser, 
                cancellationToken);
            
            return result
                ? Result<Guid>.Success(request.InvitationId)
                : Result<Guid>.Failure(["Ошибка при удалении пригалшения"], ApiErrorType.BadRequest);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка удаления приглашения");
            
            return Result<Guid>.Failure(ex);
        }
    }
}