using EventService.Domain.Abstractions.Application.Services.AppServices;
using EventService.Domain.Enums;
using EventService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventService.Application.UseCases.InvitationCases.UpdateInvitation;

public class UpdateInvitationHandler : IRequestHandler<UpdateInvitationCommand, Result<Guid>>
{
    private readonly IEventService _eventService;

    private readonly ILogger<UpdateInvitationHandler> _logger;

    public UpdateInvitationHandler(
        IEventService eventService,
        ILogger<UpdateInvitationHandler> logger
    )
    {
        _eventService = eventService;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(
        UpdateInvitationCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            bool result = await _eventService.UpdateInvitation(
                request.EventId,
                request.CurrentUser,
                request.InvitationId,
                request.RoleId,
                request.ShortDescription,
                request.Description,
                request.RequiredMember,
                request.DeadLine,
                cancellationToken
            );

            return result
                ? Result<Guid>.Success(request.InvitationId)
                : Result<Guid>.Failure(
                    ["Ошибка при обновления пригалшения"],
                    ApiErrorType.BadRequest
                );
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка обновления приглашения");

            return Result<Guid>.Failure(ex);
        }
    }
}
