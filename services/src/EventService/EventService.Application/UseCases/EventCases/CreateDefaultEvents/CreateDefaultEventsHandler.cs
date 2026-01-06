using EventService.Application.Contract;
using EventService.Domain.Abstractions.Application.Services.AppServices;
using EventService.Domain.Abstractions.Infrastructure.Storage;
using EventService.Domain.Abstractions.Infrastructure.Transactions;
using EventService.Domain.Enums;
using EventService.Domain.Models;
using EventService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EventService.Application.UseCases.EventCases.CreateDefaultEvents;

public class CreateDefaultEventsHandler : IRequestHandler<CreateDefaultEventsCommand, Result<List<CreatedEvent>>>
{
    private readonly IEventService _eventService;

    private readonly List<DefaultEvent> _defaultEvent;
    
    private readonly ILogger<CreateDefaultEventsHandler> _logger;
    
    private readonly IStorageService _storageService;
    
    private readonly IUnitOfWork _unitOfWork;

    public CreateDefaultEventsHandler(
        IEventService eventService, 
        IOptions<DefaultEventOptions> options, 
        ILogger<CreateDefaultEventsHandler> logger, 
        IStorageService storageService,
        IUnitOfWork unitOfWork)
    {
        _eventService = eventService;
        _defaultEvent = options.Value.DefaultEvents;
        _logger = logger;
        _storageService = storageService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<List<CreatedEvent>>> Handle(
        CreateDefaultEventsCommand request, CancellationToken cancellationToken
        )
    {
        if(await _eventService.IsEventsExist(cancellationToken))
            return Result<List<CreatedEvent>>
                .Failure("Нельзя добавить дефолтные события, если база не пустая", ApiErrorType.BadRequest);
        
        await _unitOfWork.BeginTransactionAsync(cancellationToken);
        
        try
        {
            List<CreatedEvent> createdEvents = new List<CreatedEvent>();
            
            foreach (var defaultEvent in _defaultEvent)
            {
                DateTime dateStart = GenerateRandomDate().ToUniversalTime();
                
                Event result = await _eventService.CreateEvent(
                    defaultEvent.Title,
                    defaultEvent.ShortDescription,
                    defaultEvent.Description,
                    dateStart,
                    defaultEvent.LocationId,
                    defaultEvent.GenreId,
                    defaultEvent.TypeId,
                    defaultEvent.MinAge,
                    request.AuthorId,
                    cancellationToken);
                
                byte[] fileBytes = LoadImageFromAssembly(defaultEvent.ImagePath);

                await _storageService.UploadFileAsync(
                    fileBytes,
                    result.PreviewUrl,
                    "image/jpeg",
                    cancellationToken);
                
                createdEvents.Add(new CreatedEvent(result,""));
            }

            await _unitOfWork.CommitAsync(cancellationToken);
            
            return Result<List<CreatedEvent>>.Success(createdEvents);
        }
        catch (Exception ex)
        {
            await _unitOfWork.Rollback(cancellationToken);
            _logger.LogError(ex, "Ошибка при создании события");
            return Result<List<CreatedEvent>>.Failure(ex);
        }
    }
    
    private static DateTime GenerateRandomDate()
    {
        DateTime now = DateTime.Now;
        DateTime start = now.AddMonths(1);
        DateTime end = now.AddMonths(2);

        int rangeDays = (end - start).Days;
        int offset = Random.Shared.Next(rangeDays + 1);
        return start.AddDays(offset);
    }

    private static byte[] LoadImageFromAssembly(string relativePath)
    {
        string full = Path.Combine(AppContext.BaseDirectory, relativePath);

        if (!File.Exists(full))
            throw new FileNotFoundException($"Seed image not found: {full}");

        return File.ReadAllBytes(full);
    }
}