using IdentityService.Domain.Abstractions.Application.Services.UserContext;
using IdentityService.Domain.CustomExceptions;
using IdentityService.Domain.Enums;
using IdentityService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.UseCases.CatalogCases.DepartmentCases.DeleteDepartment;

public class DeleteDepartmentHandler : IRequestHandler<DeleteDepartmentCommand, Result<int>>
{
    private readonly IDepartmentService _departmentService;

    private readonly ILogger<DeleteDepartmentHandler> _logger;

    public DeleteDepartmentHandler(
        IDepartmentService departmentService,
        ILogger<DeleteDepartmentHandler> logger
    )
    {
        _departmentService = departmentService;
        _logger = logger;
    }

    public async Task<Result<int>> Handle(
        DeleteDepartmentCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            bool result = await _departmentService.DeleteDepartment(
                request.DepartmentId,
                cancellationToken
            );

            return result
                ? Result<int>.Success(request.DepartmentId)
                : Result<int>.Failure(["Ошибка удаления отдела"], ApiErrorType.BadRequest);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Произошла доменная ошибка при удалении отдела");
            return Result<int>.Failure(ex);
        }
        catch (DatabaseException ex)
        {
            _logger.LogWarning(ex, "Произошла ошибка базы данных при удалении отдела");

            if (ex.ErrorType == ApiErrorType.UnprocessableEntity)
                return Result<int>.Failure(
                    ["Нельзя удалить используемую отдел"],
                    ApiErrorType.UnprocessableEntity
                );

            return Result<int>.Failure(ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Произошла непредвиденная ошибка при удалении отдела");
            return Result<int>.Failure(ex);
        }
    }
}
