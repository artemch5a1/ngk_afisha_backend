using EventService.Application.Settings.Events;
using EventService.Domain.Abstractions.Application.Services.AppServices;
using EventService.Domain.Abstractions.Infrastructure.Storage;
using EventService.Domain.Enums;
using EventService.Domain.Models;
using EventService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EventService.Application.UseCases.EventCases.GetEventById;

public class GetEventByIdHandler : IRequestHandler<GetEventByIdQuery, Result<Event>>
{
    private readonly IEventService _eventService;

    private readonly ILogger<GetEventByIdHandler> _logger;

    private readonly IStorageService _storageService;

    private readonly EventSetting _eventSetting;

    public GetEventByIdHandler(
        IEventService eventService,
        ILogger<GetEventByIdHandler> logger,
        IStorageService storageService,
        IOptions<EventSetting> eventSetting
    )
    {
        _eventService = eventService;
        _logger = logger;
        _storageService = storageService;
        _eventSetting = eventSetting.Value;
    }

    public async Task<Result<Event>> Handle(
        GetEventByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            Event? result = await _eventService.GetEventById(request.EventId, cancellationToken);

            if (result is null)
                return Result<Event>.Failure(["Событие не найдено"], ApiErrorType.NotFound);

            string url = await _storageService.GenerateDownloadUrlAsync(
                result.PreviewUrl,
                TimeSpan.FromMinutes(_eventSetting.TimeActiveDownloadLinkInMilliSeconds)
            );

            result.SetDownloadUrl(url);

            return Result<Event>.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении события по id");
            return Result<Event>.Failure(ex);
        }
    }
}
