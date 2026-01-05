using EventService.Domain.Abstractions.Application.Services.AppServices;
using EventService.Domain.Enums;
using EventService.Domain.Models;
using EventService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventService.Application.UseCases.EventRoleUseCases.GetEventRoleById;

public class GetEventRoleByIdHandler : IRequestHandler<GetEventRoleByIdQuery, Result<EventRole>>
{
    private readonly IEventRoleService _eventRoleService;

    private readonly ILogger<GetEventRoleByIdHandler> _logger;


    public GetEventRoleByIdHandler(IEventRoleService eventRoleService, ILogger<GetEventRoleByIdHandler> logger)
    {
        _eventRoleService = eventRoleService;
        _logger = logger;
    }

    public async Task<Result<EventRole>> Handle(GetEventRoleByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            EventRole? eventRole = await _eventRoleService.GetEventRoleById(request.EventRoleId, cancellationToken);

            if (eventRole is null)
                return Result<EventRole>.Failure(["Такой роли не существует"], ApiErrorType.NotFound);

            return Result<EventRole>.Success(eventRole);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка при получении роли по id");
            return Result<EventRole>.Failure(ex);
        }
    }
}