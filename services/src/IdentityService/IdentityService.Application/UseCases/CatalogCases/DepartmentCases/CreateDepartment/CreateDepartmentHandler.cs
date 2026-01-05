using IdentityService.Domain.Abstractions.Application.Services.UserContext;
using IdentityService.Domain.CustomExceptions;
using IdentityService.Domain.Models.UserContext;
using IdentityService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.UseCases.CatalogCases.DepartmentCases.CreateDepartment;

public class CreateDepartmentHandler : IRequestHandler<CreateDepartmentCommand, Result<Department>>
{
    private readonly IDepartmentService _departmentService;

    private readonly ILogger<CreateDepartmentHandler> _logger;

    public CreateDepartmentHandler(
        IDepartmentService departmentService, 
        ILogger<CreateDepartmentHandler> logger)
    {
        _departmentService = departmentService;
        _logger = logger;
    }

    public async Task<Result<Department>> Handle(CreateDepartmentCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Department result = await _departmentService.CreateDepartment(request.DepartmentTitle, cancellationToken);

            return Result<Department>.Success(result);
        }
        catch (DatabaseException ex)
        {
            _logger.LogWarning(ex, "Ошибка базы данных при создании отдела");

            return Result<Department>.Failure(ex);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Доменная ошибка при создании отдела");

            return Result<Department>.Failure(ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Неизвестная ошибка при создании отдела");

            return Result<Department>.Failure(ex);
        }
    }
}