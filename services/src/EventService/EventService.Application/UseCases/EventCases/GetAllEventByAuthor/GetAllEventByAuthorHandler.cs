using EventService.Application.Settings.Events;
using EventService.Domain.Abstractions.Application.Services.AppServices;
using EventService.Domain.Abstractions.Infrastructure.Storage;
using EventService.Domain.Models;
using EventService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EventService.Application.UseCases.EventCases.GetAllEventByAuthor;

public class GetAllEventByAuthorHandler : IRequestHandler<GetAllEventByAuthorQuery, Result<List<Event>>>
{
    private readonly IEventService _eventService;

    private readonly ILogger<GetAllEventByAuthorHandler> _logger;

    private readonly IStorageService _storageService;

    private readonly EventSetting _eventSetting;
    
    public GetAllEventByAuthorHandler(
        IEventService eventService, 
        ILogger<GetAllEventByAuthorHandler> logger,
        IStorageService storageService, 
        IOptions<EventSetting> eventSetting)
    {
        _eventService = eventService;
        _logger = logger;
        _storageService = storageService;
        _eventSetting = eventSetting.Value;
    }


    public async Task<Result<List<Event>>> Handle(GetAllEventByAuthorQuery request, CancellationToken cancellationToken)
    {
        try
        {
            List<Event> result = await _eventService.GetAllEventByAuthorId(request.AuthorId ,request.Contract, cancellationToken);

            foreach (Event @event in result)
            {
                string url = await _storageService
                    .GenerateDownloadUrlAsync(@event.PreviewUrl, 
                        TimeSpan.FromMinutes(_eventSetting.TimeActiveDownloadLinkInMilliSeconds));
                
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