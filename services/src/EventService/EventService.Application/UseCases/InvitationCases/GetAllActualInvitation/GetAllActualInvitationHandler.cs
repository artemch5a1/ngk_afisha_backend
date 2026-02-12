using EventService.Application.Settings.Events;
using EventService.Domain.Abstractions.Application.Services.AppServices;
using EventService.Domain.Abstractions.Infrastructure.Storage;
using EventService.Domain.Models;
using EventService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EventService.Application.UseCases.InvitationCases.GetAllActualInvitation;

public class GetAllActualInvitationHandler
    : IRequestHandler<GetAllActualInvitationQuery, Result<List<Invitation>>>
{
    private readonly IInvitationService _invitationService;

    private readonly ILogger<GetAllActualInvitationHandler> _logger;
    private readonly IStorageService _storageService;

    private readonly EventSetting _eventSetting;

    public GetAllActualInvitationHandler(
        IInvitationService invitationService,
        ILogger<GetAllActualInvitationHandler> logger,
        IStorageService storageService,
        IOptions<EventSetting> eventSetting
    )
    {
        _invitationService = invitationService;
        _logger = logger;
        _storageService = storageService;
        _eventSetting = eventSetting.Value;
    }

    public async Task<Result<List<Invitation>>> Handle(
        GetAllActualInvitationQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            List<Invitation> invitations = await _invitationService.GetAllActualInvitation(
                request.Contract,
                cancellationToken
            );

            foreach (Event @event in invitations.Select(i => i.Event))
            {
                string url = await _storageService.GenerateDownloadUrlAsync(
                    @event.PreviewUrl,
                    TimeSpan.FromMinutes(_eventSetting.TimeActiveDownloadLinkInMilliSeconds)
                );

                @event.SetDownloadUrl(url);
            }

            return Result<List<Invitation>>.Success(invitations);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка при получении пригалшений");

            return Result<List<Invitation>>.Failure(ex);
        }
    }
}
