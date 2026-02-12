using IdentityService.Domain.Abstractions.Application.Services.UserContext;
using IdentityService.Domain.CustomExceptions;
using IdentityService.Domain.Models.UserContext;
using IdentityService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.UseCases.CatalogCases.GroupCases.GetAllGroup;

public class GetAllGroupHandler : IRequestHandler<GetAllGroupQuery, Result<List<Group>>>
{
    private readonly IGroupService _groupService;

    private readonly ILogger<GetAllGroupHandler> _logger;

    public GetAllGroupHandler(IGroupService groupService, ILogger<GetAllGroupHandler> logger)
    {
        _groupService = groupService;
        _logger = logger;
    }

    public async Task<Result<List<Group>>> Handle(
        GetAllGroupQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            List<Group> result = await _groupService.GetAllGroup(cancellationToken);

            return Result<List<Group>>.Success(result);
        }
        catch (DatabaseException ex)
        {
            _logger.LogWarning(ex, "Ошибка базы данных при получении всех групп");

            return Result<List<Group>>.Failure(ex);
            ;
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Доменная ошибка при получении всех групп");

            return Result<List<Group>>.Failure(ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Неизвестная ошибка при получении всех групп");

            return Result<List<Group>>.Failure(ex);
        }
    }
}
