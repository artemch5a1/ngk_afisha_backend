using IdentityService.Domain.Abstractions.Application.Services.UserContext;
using IdentityService.Domain.CustomExceptions;
using IdentityService.Domain.Enums;
using IdentityService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.UseCases.CatalogCases.GroupCases.UpdateGroup;

public class UpdateGroupHandler : IRequestHandler<UpdateGroupCommand, Result<int>>
{
    private readonly IGroupService _groupService;

    private readonly ILogger<UpdateGroupHandler> _logger;
    
    public UpdateGroupHandler(IGroupService groupService, ILogger<UpdateGroupHandler> logger)
    {
        _groupService = groupService;
        _logger = logger;
    }


    public async Task<Result<int>> Handle(UpdateGroupCommand request, CancellationToken cancellationToken)
    {
        try
        {
            bool result = await _groupService.UpdateGroup(request.GroupId, request.Course, request.NumberGroup,
                request.SpecialtyId, cancellationToken);

            return result
                ? Result<int>.Success(request.GroupId)
                : Result<int>.Failure(["Ошибка обновления группы"], ApiErrorType.BadRequest);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Произошла доменная ошибка при обновлении группы");
            return Result<int>.Failure(ex);
        }
        catch (DatabaseException ex)
        {
            _logger.LogWarning(ex, "Произошла ошибка базы данных при обновлении группы");
            return Result<int>.Failure(ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Произошла непредвиденная ошибка при обновлении группы");
            return Result<int>.Failure(ex);
        }
    }
}