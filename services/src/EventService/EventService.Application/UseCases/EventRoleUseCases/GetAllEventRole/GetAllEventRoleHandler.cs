using EventService.Domain.Abstractions.Application.Services.AppServices;
using EventService.Domain.Models;
using EventService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventService.Application.UseCases.EventRoleUseCases.GetAllEventRole;

public class GetAllEventRoleHandler : IRequestHandler<GetAllEventRoleQuery, Result<List<EventRole>>>
{
    private readonly IEventRoleService _eventRoleService;

    private readonly ILogger<GetAllEventRoleHandler> _logger;

    public GetAllEventRoleHandler(
        IEventRoleService eventRoleService,
        ILogger<GetAllEventRoleHandler> logger
    )
    {
        _eventRoleService = eventRoleService;
        _logger = logger;
    }

    public async Task<Result<List<EventRole>>> Handle(
        GetAllEventRoleQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            List<EventRole> eventRoles = await _eventRoleService.GetAllEventRoles(
                request.Contract,
                cancellationToken
            );

            return Result<List<EventRole>>.Success(eventRoles);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка при получении всех ролей события");
            return Result<List<EventRole>>.Failure(ex);
        }
    }
}
