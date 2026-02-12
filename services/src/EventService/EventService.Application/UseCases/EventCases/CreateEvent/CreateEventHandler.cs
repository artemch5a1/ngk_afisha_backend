using EventService.Application.Contract;
using EventService.Application.Settings.Events;
using EventService.Domain.Abstractions.Application.Services.AppServices;
using EventService.Domain.Abstractions.Infrastructure.Storage;
using EventService.Domain.Models;
using EventService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EventService.Application.UseCases.EventCases.CreateEvent;

public class CreateEventHandler : IRequestHandler<CreateEventCommand, Result<CreatedEvent>>
{
    private readonly IEventService _eventService;

    private readonly ILogger<CreateEventHandler> _logger;

    private readonly IStorageService _storageService;

    private readonly EventSetting _eventSetting;

    public CreateEventHandler(
        IEventService eventService,
        ILogger<CreateEventHandler> logger,
        IStorageService storageService,
        IOptions<EventSetting> eventOptions
    )
    {
        _eventService = eventService;
        _logger = logger;
        _storageService = storageService;
        _eventSetting = eventOptions.Value;
    }

    public async Task<Result<CreatedEvent>> Handle(
        CreateEventCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            Event result = await _eventService.CreateEvent(
                request.Title,
                request.ShortDescription,
                request.Description,
                request.DateStart,
                request.LocationId,
                request.GenreId,
                request.TypeId,
                request.MinAge,
                request.Author,
                cancellationToken
            );

            string url = await _storageService.GenerateUploadUrlAsync(
                result.PreviewUrl,
                TimeSpan.FromMinutes(_eventSetting.TimeActiveUploadLinkInMilliSeconds)
            );

            return Result<CreatedEvent>.Success(new CreatedEvent(result, url));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при создании события");
            return Result<CreatedEvent>.Failure(ex);
        }
    }
}
