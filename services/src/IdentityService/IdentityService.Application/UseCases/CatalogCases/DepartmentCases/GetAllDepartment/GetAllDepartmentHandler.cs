using IdentityService.Domain.Abstractions.Application.Services.UserContext;
using IdentityService.Domain.CustomExceptions;
using IdentityService.Domain.Models.UserContext;
using IdentityService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.UseCases.CatalogCases.DepartmentCases.GetAllDepartment;

public class GetAllDepartmentHandler
    : IRequestHandler<GetAllDepartmentQuery, Result<List<Department>>>
{
    private readonly IDepartmentService _departmentService;

    private readonly ILogger<GetAllDepartmentHandler> _logger;

    public GetAllDepartmentHandler(
        IDepartmentService departmentService,
        ILogger<GetAllDepartmentHandler> logger
    )
    {
        _departmentService = departmentService;
        _logger = logger;
    }

    public async Task<Result<List<Department>>> Handle(
        GetAllDepartmentQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            List<Department> result = await _departmentService.GetAllDepartment(cancellationToken);

            return Result<List<Department>>.Success(result);
        }
        catch (DatabaseException ex)
        {
            _logger.LogWarning(ex, "Ошибка базы данных при получении всех отделов");

            return Result<List<Department>>.Failure(ex);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Доменная ошибка при получении всех отделов");

            return Result<List<Department>>.Failure(ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Неизвестная ошибка при получении всех отделов");

            return Result<List<Department>>.Failure(ex);
        }
    }
}
