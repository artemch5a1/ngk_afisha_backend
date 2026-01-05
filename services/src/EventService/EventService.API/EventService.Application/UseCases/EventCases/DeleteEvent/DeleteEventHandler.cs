using EventService.Domain.Abstractions.Application.Services.AppServices;
using EventService.Domain.Abstractions.Infrastructure.Storage;
using EventService.Domain.CustomExceptions;
using EventService.Domain.Enums;
using EventService.Domain.Models;
using EventService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventService.Application.UseCases.EventCases.DeleteEvent;

public class DeleteEventHandler : IRequestHandler<DeleteEventCommand, Result<Guid>>
{
    private readonly IEventService _eventService;

    private readonly ILogger<DeleteEventHandler> _logger;
    private readonly IStorageService _storageService;

    public DeleteEventHandler(IEventService eventService, ILogger<DeleteEventHandler> logger, IStorageService storageService)
    {
        _eventService = eventService;
        _logger = logger;
        _storageService = storageService;
    }


    public async Task<Result<Guid>> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Event? @event = await _eventService.GetEventById(request.EventId, cancellationToken);

            if (@event is null)
                throw new NotFoundException("Событие", request.EventId);

            string key = @event.PreviewUrl;
            
            bool result = await _eventService.DeleteEvent(request.EventId, cancellationToken);

            await _storageService.DeleteAsync(key);
            
            return result ? 
                Result<Guid>.Success(request.EventId) : 
                Result<Guid>.Failure(["Ошибка удаления"], ApiErrorType.BadRequest);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при удалении события");
            return Result<Guid>.Failure(ex);
        }
    }
}