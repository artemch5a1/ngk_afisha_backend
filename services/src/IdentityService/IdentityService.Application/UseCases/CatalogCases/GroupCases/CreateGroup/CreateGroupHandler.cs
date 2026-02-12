using IdentityService.Domain.Abstractions.Application.Services.UserContext;
using IdentityService.Domain.CustomExceptions;
using IdentityService.Domain.Models.UserContext;
using IdentityService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.UseCases.CatalogCases.GroupCases.CreateGroup;

public class CreateGroupHandler : IRequestHandler<CreateGroupCommand, Result<Group>>
{
    private readonly IGroupService _groupService;

    private readonly ILogger<CreateGroupHandler> _logger;

    public CreateGroupHandler(IGroupService groupService, ILogger<CreateGroupHandler> logger)
    {
        _groupService = groupService;
        _logger = logger;
    }

    public async Task<Result<Group>> Handle(
        CreateGroupCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            Group result = await _groupService.CreateGroup(
                request.Course,
                request.NumberGroup,
                request.SpecialtyId,
                cancellationToken
            );

            return Result<Group>.Success(result);
        }
        catch (DatabaseException ex)
        {
            _logger.LogWarning(ex, "Ошибка базы данных при создании группы");

            return Result<Group>.Failure(ex);
            ;
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Доменная ошибка при создании группы");

            return Result<Group>.Failure(ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Неизвестная ошибка при создании группы");

            return Result<Group>.Failure(ex);
        }
    }
}
