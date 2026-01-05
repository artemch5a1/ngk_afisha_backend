using EventService.Application.UseCases.InvitationCases.CancelRequestOnEventInvitation;
using EventService.Domain.Abstractions.Application.Services.AppServices;
using EventService.Domain.Enums;
using EventService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventService.Application.UseCases.InvitationCases.TakeRequestOnEventInvitation;

public class CancelRequestHandler : IRequestHandler<CancelRequestCommand, Result<Guid>>
{
    private readonly IEventService _eventService;

    private readonly ILogger<TakeRequestHandler> _logger;


    public CancelRequestHandler(
        IEventService eventService, 
        ILogger<TakeRequestHandler> logger)
    {
        _eventService = eventService;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(CancelRequestCommand request, CancellationToken cancellationToken)
    {
        try
        {
            bool result = await _eventService.CancelRequestOnInvitation(
                request.EventId, 
                request.InvitationId, 
                request.StudentId,
                cancellationToken);

            return result
                ? Result<Guid>.Success(request.InvitationId)
                : Result<Guid>.Failure(["Ошибка при отмене заявки"], ApiErrorType.BadRequest);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка при отмене заявки на приглашение");
            return Result<Guid>.Failure(ex);
        }
    }
}