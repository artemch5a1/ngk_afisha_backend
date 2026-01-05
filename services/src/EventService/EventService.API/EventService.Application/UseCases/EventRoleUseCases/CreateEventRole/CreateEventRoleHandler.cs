using EventService.Domain.Abstractions.Application.Services.AppServices;
using EventService.Domain.Models;
using EventService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventService.Application.UseCases.EventRoleUseCases.CreateEventRole;

public class CreateEventRoleHandler : IRequestHandler<CreateEventRoleCommand, Result<EventRole>>
{
    private readonly IEventRoleService _eventRoleService;

    private readonly ILogger<CreateEventRoleHandler> _logger;


    public CreateEventRoleHandler(IEventRoleService eventRoleService, ILogger<CreateEventRoleHandler> logger)
    {
        _eventRoleService = eventRoleService;
        _logger = logger;
    }

    public async Task<Result<EventRole>> Handle(CreateEventRoleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            EventRole eventRole =
                await _eventRoleService.CreateEventRole(request.Title, request.Description, cancellationToken);

            return Result<EventRole>.Success(eventRole);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка при создании роли");
            return Result<EventRole>.Failure(ex);
        }
    }
}