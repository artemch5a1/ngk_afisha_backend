using IdentityService.Domain.Abstractions.Application.Services.UserContext;
using IdentityService.Domain.CustomExceptions;
using IdentityService.Domain.Enums;
using IdentityService.Domain.Models.UserContext;
using IdentityService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.UseCases.CatalogCases.GroupCases.GetGroupById;

public class GetGroupByIdHandler : IRequestHandler<GetGroupByIdQuery, Result<Group>>
{
    private readonly IGroupService _groupService;

    private readonly ILogger<GetGroupByIdHandler> _logger;

    public GetGroupByIdHandler(IGroupService groupService, ILogger<GetGroupByIdHandler> logger)
    {
        _groupService = groupService;
        _logger = logger;
    }

    public async Task<Result<Group>> Handle(
        GetGroupByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            Group? result = await _groupService.GetGroupById(request.GroupId, cancellationToken);

            if (result is null)
                return Result<Group>.Failure("Группа не найдена", ApiErrorType.NotFound);

            return Result<Group>.Success(result);
        }
        catch (DatabaseException ex)
        {
            _logger.LogWarning(ex, "Ошибка базы данных при получении группы по id");

            return Result<Group>.Failure(ex);
            ;
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Доменная ошибка при получении группы по id");

            return Result<Group>.Failure(ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Неизвестная ошибка при получении группы по id");

            return Result<Group>.Failure(ex);
        }
    }
}
