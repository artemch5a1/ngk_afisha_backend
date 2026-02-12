using IdentityService.Domain.Abstractions.Application.Services.UserContext;
using IdentityService.Domain.CustomExceptions;
using IdentityService.Domain.Enums;
using IdentityService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.UseCases.CatalogCases.DepartmentCases.UpdateDepartment;

public class UpdateDepartmentHandler : IRequestHandler<UpdateDepartmentCommand, Result<int>>
{
    private readonly IDepartmentService _departmentService;

    private readonly ILogger<UpdateDepartmentHandler> _logger;

    public UpdateDepartmentHandler(
        IDepartmentService departmentService,
        ILogger<UpdateDepartmentHandler> logger
    )
    {
        _departmentService = departmentService;
        _logger = logger;
    }

    public async Task<Result<int>> Handle(
        UpdateDepartmentCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            bool result = await _departmentService.UpdateDepartment(
                request.DepartmentId,
                request.DepartmentTitle,
                cancellationToken
            );

            return result
                ? Result<int>.Success(request.DepartmentId)
                : Result<int>.Failure(["Ошибка обновления отдела"], ApiErrorType.BadRequest);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Произошла доменная ошибка при обновлении отдела");
            return Result<int>.Failure(ex);
        }
        catch (DatabaseException ex)
        {
            _logger.LogWarning(ex, "Произошла ошибка базы данных при обновлении отдела");
            return Result<int>.Failure(ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Произошла непредвиденная ошибка при обновлении отдела");
            return Result<int>.Failure(ex);
        }
    }
}
