using EventService.Domain.Abstractions.Application.Services.AppServices;
using EventService.Domain.Enums;
using EventService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventService.Application.UseCases.EventRoleUseCases.DeleteEventRole;

public class DeleteEventRoleHandler : IRequestHandler<DeleteEventRoleCommand, Result<int>>
{
    private readonly IEventRoleService _eventRoleService;

    private readonly ILogger<DeleteEventRoleHandler> _logger;

    public DeleteEventRoleHandler(
        IEventRoleService eventRoleService,
        ILogger<DeleteEventRoleHandler> logger
    )
    {
        _eventRoleService = eventRoleService;
        _logger = logger;
    }

    public async Task<Result<int>> Handle(
        DeleteEventRoleCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            bool result = await _eventRoleService.DeleteEventRole(
                request.EventRoleId,
                cancellationToken
            );

            return result
                ? Result<int>.Success(request.EventRoleId)
                : Result<int>.Failure(["Ошибка при удалении"], ApiErrorType.BadRequest);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка при попытке удалить роль");
            return Result<int>.Failure(ex);
        }
    }
}
