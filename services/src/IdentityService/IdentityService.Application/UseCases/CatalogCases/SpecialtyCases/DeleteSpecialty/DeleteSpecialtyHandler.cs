using IdentityService.Domain.Abstractions.Application.Services.UserContext;
using IdentityService.Domain.CustomExceptions;
using IdentityService.Domain.Enums;
using IdentityService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.UseCases.CatalogCases.SpecialtyCases.DeleteSpecialty;

public class DeleteSpecialtyHandler : IRequestHandler<DeleteSpecialtyCommand, Result<int>>
{
    private readonly ISpecialtyService _specialtyService;

    private readonly ILogger<DeleteSpecialtyHandler> _logger;

    public DeleteSpecialtyHandler(ISpecialtyService specialtyService, ILogger<DeleteSpecialtyHandler> logger)
    {
        _specialtyService = specialtyService;
        _logger = logger;
    }

    public async Task<Result<int>> Handle(DeleteSpecialtyCommand request, CancellationToken cancellationToken)
    {
        try
        {
            bool result = await _specialtyService.DeleteSpecialty(request.SpecialtyId, cancellationToken);
            
            if (result)
                return Result<int>.Success(request.SpecialtyId);
            
            return Result<int>.Failure(["Не удалось удалить специальность"], ApiErrorType.BadRequest);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Доменная ошибка при удалении специальности");

            return Result<int>.Failure(ex);
        }
        catch (DatabaseException ex)
        {
            _logger.LogWarning(ex, "Ошибка базы данных при удалении специальности");
            
            if(ex.ErrorType == ApiErrorType.UnprocessableEntity)
                return Result<int>.Failure(["Нельзя удалить используемую специальность"], ApiErrorType.UnprocessableEntity);
            
            return Result<int>.Failure(ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Непредвиденная ошибка при удалении специальности");

            return Result<int>.Failure(ex);
        }
    }
}