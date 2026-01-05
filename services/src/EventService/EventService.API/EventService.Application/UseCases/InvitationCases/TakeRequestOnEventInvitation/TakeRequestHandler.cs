using EventService.Domain.Abstractions.Application.Services.AppServices;
using EventService.Domain.Enums;
using EventService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventService.Application.UseCases.InvitationCases.TakeRequestOnEventInvitation;

public class TakeRequestHandler : IRequestHandler<TakeRequestCommand, Result<Guid>>
{
    private readonly IEventService _eventService;

    private readonly ILogger<TakeRequestHandler> _logger;


    public TakeRequestHandler(
        IEventService eventService, 
        ILogger<TakeRequestHandler> logger)
    {
        _eventService = eventService;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(TakeRequestCommand request, CancellationToken cancellationToken)
    {
        try
        {
            bool result = await _eventService.TakeRequestOnInvitation(
                request.EventId, 
                request.InvitationId, 
                request.StudentId,
                cancellationToken);

            return result
                ? Result<Guid>.Success(request.InvitationId)
                : Result<Guid>.Failure(["Ошибка при состалвении заявки"], ApiErrorType.BadRequest);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка при создании заявки на приглашение");
            return Result<Guid>.Failure(ex);
        }
    }
}