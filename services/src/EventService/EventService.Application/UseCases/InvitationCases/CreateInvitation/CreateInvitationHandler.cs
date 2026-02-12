using EventService.Domain.Abstractions.Application.Services.AppServices;
using EventService.Domain.Models;
using EventService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventService.Application.UseCases.InvitationCases.CreateInvitation;

public class CreateInvitationHandler : IRequestHandler<CreateInvitationCommand, Result<Invitation>>
{
    private readonly IEventService _eventService;

    private readonly ILogger<CreateInvitationHandler> _logger;

    public CreateInvitationHandler(
        IEventService eventService,
        ILogger<CreateInvitationHandler> logger
    )
    {
        _eventService = eventService;
        _logger = logger;
    }

    public async Task<Result<Invitation>> Handle(
        CreateInvitationCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            Invitation invitation = await _eventService.CreateInvitation(
                request.EventId,
                request.CurrentUser,
                request.RoleId,
                request.ShortDescription,
                request.Description,
                request.RequiredMember,
                request.DeadLine,
                cancellationToken
            );

            return Result<Invitation>.Success(invitation);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Произошла ошибка во время создания приглашения");
            return Result<Invitation>.Failure(ex);
        }
    }
}
