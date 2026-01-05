using EventService.Domain.Abstractions.Application.Services.AppServices;
using EventService.Domain.Enums;
using EventService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventService.Application.UseCases.EventRoleUseCases.UpdateEventRole;

public class UpdateEventRoleHandler : IRequestHandler<UpdateEventRoleCommand, Result<int>>
{
    private readonly IEventRoleService _eventRoleService;

    private readonly ILogger<UpdateEventRoleHandler> _logger;


    public UpdateEventRoleHandler(IEventRoleService eventRoleService, ILogger<UpdateEventRoleHandler> logger)
    {
        _eventRoleService = eventRoleService;
        _logger = logger;
    }


    public async Task<Result<int>> Handle(UpdateEventRoleCommand request, CancellationToken cancellationToken)
    {
        try
        {
            bool result =
                await _eventRoleService.UpdateEventRole(request.EventRoleId, request.Title, request.Description, cancellationToken);
            
            return result
                ? Result<int>.Success(request.EventRoleId)
                : Result<int>.Failure(["Ошибка при обновлении"], ApiErrorType.BadRequest);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка при обновлении роли");

            return Result<int>.Failure(ex);
        }
    }
}