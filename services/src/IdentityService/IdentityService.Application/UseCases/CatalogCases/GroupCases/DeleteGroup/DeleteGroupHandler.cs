using IdentityService.Domain.Abstractions.Application.Services.UserContext;
using IdentityService.Domain.CustomExceptions;
using IdentityService.Domain.Enums;
using IdentityService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.UseCases.CatalogCases.GroupCases.DeleteGroup;

public class DeleteGroupHandler : IRequestHandler<DeleteGroupCommand, Result<int>>
{
    private readonly IGroupService _groupService;

    private readonly ILogger<DeleteGroupHandler> _logger;

    public DeleteGroupHandler(IGroupService groupService, ILogger<DeleteGroupHandler> logger)
    {
        _groupService = groupService;
        _logger = logger;
    }

    public async Task<Result<int>> Handle(
        DeleteGroupCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            bool result = await _groupService.DeleteGroup(request.GroupId, cancellationToken);

            return result
                ? Result<int>.Success(request.GroupId)
                : Result<int>.Failure(["Ошибка удаления группы"], ApiErrorType.BadRequest);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Произошла доменная ошибка при удалении группы");
            return Result<int>.Failure(ex);
        }
        catch (DatabaseException ex)
        {
            _logger.LogWarning(ex, "Произошла ошибка базы данных при удалении группы");

            if (ex.ErrorType == ApiErrorType.UnprocessableEntity)
                return Result<int>.Failure(
                    ["Нельзя удалить используемую группу"],
                    ApiErrorType.UnprocessableEntity
                );

            return Result<int>.Failure(ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Произошла непредвиденная ошибка при удалении группы");
            return Result<int>.Failure(ex);
        }
    }
}
