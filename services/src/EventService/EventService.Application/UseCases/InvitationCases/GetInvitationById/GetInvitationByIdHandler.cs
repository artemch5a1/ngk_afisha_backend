using EventService.Application.Settings.Events;
using EventService.Domain.Abstractions.Application.Services.AppServices;
using EventService.Domain.Abstractions.Infrastructure.Storage;
using EventService.Domain.Enums;
using EventService.Domain.Models;
using EventService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EventService.Application.UseCases.InvitationCases.GetInvitationById;

public class GetInvitationByIdHandler : IRequestHandler<GetInvitationByIdQuery, Result<Invitation>>
{
    private readonly IInvitationService _invitationService;

    private readonly ILogger<GetInvitationByIdHandler> _logger;

    private readonly IStorageService _storageService;

    private readonly EventSetting _eventSetting;

    public GetInvitationByIdHandler(
        IInvitationService invitationService,
        ILogger<GetInvitationByIdHandler> logger,
        IStorageService storageService,
        IOptions<EventSetting> eventSetting
    )
    {
        _invitationService = invitationService;
        _logger = logger;
        _storageService = storageService;
        _eventSetting = eventSetting.Value;
    }

    public async Task<Result<Invitation>> Handle(
        GetInvitationByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            Invitation? location = await _invitationService.GetInvitationById(
                request.InvitationId,
                cancellationToken
            );

            if (location is null)
                return Result<Invitation>.Failure(
                    ["Приглашение не найдено"],
                    ApiErrorType.NotFound
                );

            string url = await _storageService.GenerateDownloadUrlAsync(
                location.Event.PreviewUrl,
                TimeSpan.FromMinutes(_eventSetting.TimeActiveDownloadLinkInMilliSeconds)
            );

            location.Event.SetDownloadUrl(url);

            return Result<Invitation>.Success(location);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка при получении приглашения по id");

            return Result<Invitation>.Failure(ex);
        }
    }
}
