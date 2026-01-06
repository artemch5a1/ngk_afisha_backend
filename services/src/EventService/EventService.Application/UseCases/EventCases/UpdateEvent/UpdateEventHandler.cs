using EventService.Application.Contract;
using EventService.Application.Settings.Events;
using EventService.Domain.Abstractions.Application.Services.AppServices;
using EventService.Domain.Abstractions.Infrastructure.Storage;
using EventService.Domain.Models;
using EventService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EventService.Application.UseCases.EventCases.UpdateEvent;

public class UpdateEventHandler : IRequestHandler<UpdateEventCommand, Result<UpdatedEvent>>
{
    private readonly IEventService _eventService;

    private readonly ILogger<UpdateEventHandler> _logger;

    private readonly IStorageService _storageService;

    private readonly EventSetting _eventSetting;
    
    public UpdateEventHandler(
        IEventService eventService, 
        ILogger<UpdateEventHandler> logger, 
        IStorageService storageService,
        IOptions<EventSetting> eventOptions)
    {
        _eventService = eventService;
        _logger = logger;
        _storageService = storageService;
        _eventSetting = eventOptions.Value;
    }


    public async Task<Result<UpdatedEvent>> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Event result = await _eventService.UpdateEvent(
                request.CurrentUser,
                request.EventId,
                request.Title,
                request.ShortDescription,
                request.Description,
                request.DateStart,
                request.LocationId,
                request.GenreId,
                request.TypeId,
                request.MinAge,
                cancellationToken);

            string url = await _storageService.GenerateUploadUrlAsync(result.PreviewUrl, 
                TimeSpan.FromMinutes(_eventSetting.TimeActiveUploadLinkInMilliSeconds));
            
            return Result<UpdatedEvent>.Success(new UpdatedEvent(result, url));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при создании события");
            return Result<UpdatedEvent>.Failure(ex);
        }
    }
}