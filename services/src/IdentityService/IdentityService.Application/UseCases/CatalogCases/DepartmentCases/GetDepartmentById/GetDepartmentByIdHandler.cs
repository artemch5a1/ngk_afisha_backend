using IdentityService.Domain.Abstractions.Application.Services.UserContext;
using IdentityService.Domain.CustomExceptions;
using IdentityService.Domain.Enums;
using IdentityService.Domain.Models.UserContext;
using IdentityService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.UseCases.CatalogCases.DepartmentCases.GetDepartmentById;

public class GetDepartmentByIdHandler : IRequestHandler<GetDepartmentByIdQuery, Result<Department>>
{
    private readonly IDepartmentService _departmentService;

    private readonly ILogger<GetDepartmentByIdHandler> _logger;

    public GetDepartmentByIdHandler(
        IDepartmentService departmentService, 
        ILogger<GetDepartmentByIdHandler> logger)
    {
        _departmentService = departmentService;
        _logger = logger;
    }

    public async Task<Result<Department>> Handle(GetDepartmentByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            Department? result = await _departmentService.GetDepartmentById(request.DepartmentId, cancellationToken);

            if (result is null)
                return Result<Department>.Failure("Отдел не найден", ApiErrorType.NotFound);

            return Result<Department>.Success(result);
        }
        catch (DatabaseException ex)
        {
            _logger.LogWarning(ex, "Ошибка базы данных при получении отдела по id");

            return Result<Department>.Failure(ex);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Доменная ошибка при получении отдела по id");

            return Result<Department>.Failure(ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Неизвестная ошибка при получении отдела по id");

            return Result<Department>.Failure(ex);
        }
    }
}