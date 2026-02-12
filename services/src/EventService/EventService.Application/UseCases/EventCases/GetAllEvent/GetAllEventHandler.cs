using EventService.Application.Settings.Events;
using EventService.Application.Specifications.Events;
using EventService.Domain.Abstractions.Application.Services.AppServices;
using EventService.Domain.Abstractions.Infrastructure.Storage;
using EventService.Domain.Models;
using EventService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EventService.Application.UseCases.EventCases.GetAllEvent;

public class GetAllEventHandler : IRequestHandler<GetAllEventQuery, Result<List<Event>>>
{
    private readonly IEventService _eventService;

    private readonly ILogger<GetAllEventHandler> _logger;

    private readonly IStorageService _storageService;

    private readonly EventSetting _eventSetting;

    public GetAllEventHandler(
        IEventService eventService,
        ILogger<GetAllEventHandler> logger,
        IStorageService storageService,
        IOptions<EventSetting> eventSetting
    )
    {
        _eventService = eventService;
        _logger = logger;
        _storageService = storageService;
        _eventSetting = eventSetting.Value;
    }

    public async Task<Result<List<Event>>> Handle(
        GetAllEventQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            List<Event> result = await _eventService.GetAllEvent(
                new UpcomingEventsSpecification(),
                request.Contract,
                cancellationToken
            );

            foreach (Event @event in result)
            {
                string url = await _storageService.GenerateDownloadUrlAsync(
                    @event.PreviewUrl,
                    TimeSpan.FromMinutes(_eventSetting.TimeActiveDownloadLinkInMilliSeconds)
                );

                @event.SetDownloadUrl(url);
            }

            return Result<List<Event>>.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении событий");
            return Result<List<Event>>.Failure(ex);
        }
    }
}
