using EventService.Domain.Abstractions.Application.Services.AppServices;
using EventService.Domain.Enums;
using EventService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventService.Application.UseCases.InvitationCases.AcceptRequestOnEventInvitation;

public class AcceptRequestHandler : IRequestHandler<AcceptRequestCommand, Result<Guid>>
{
    private readonly IEventService _eventService;

    private readonly ILogger<AcceptRequestHandler> _logger;


    public AcceptRequestHandler(
        IEventService eventService, 
        ILogger<AcceptRequestHandler> logger)
    {
        _eventService = eventService;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(AcceptRequestCommand request, CancellationToken cancellationToken)
    {
        try
        {
            bool result = await _eventService.AcceptRequestOnInvitation(
                request.EventId,
                request.InvitationId,
                request.StudentId,
                request.CurrentUser,
                cancellationToken
            );
            
            return result
                ? Result<Guid>.Success(request.InvitationId)
                : Result<Guid>.Failure(["Ошибка при одобрении заявки"], ApiErrorType.BadRequest);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка принятия заявки");
            
            return Result<Guid>.Failure(ex);
        }
    }
}