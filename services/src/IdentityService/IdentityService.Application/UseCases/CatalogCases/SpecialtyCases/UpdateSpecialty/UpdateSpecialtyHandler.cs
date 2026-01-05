using IdentityService.Domain.Abstractions.Application.Services.UserContext;
using IdentityService.Domain.CustomExceptions;
using IdentityService.Domain.Enums;
using IdentityService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.UseCases.CatalogCases.SpecialtyCases.UpdateSpecialty;

public class UpdateSpecialtyHandler : IRequestHandler<UpdateSpecialtyCommand, Result<int>>
{
    private readonly ISpecialtyService _specialtyService;

    private readonly ILogger<UpdateSpecialtyHandler> _logger;


    public UpdateSpecialtyHandler(ISpecialtyService specialtyService, ILogger<UpdateSpecialtyHandler> logger)
    {
        _specialtyService = specialtyService;
        _logger = logger;
    }


    public async Task<Result<int>> Handle(UpdateSpecialtyCommand request, CancellationToken cancellationToken)
    {
        try
        {
            bool result = await _specialtyService.UpdateSpecialty(request.SpecialtyId, request.NewSpecialtyTitle, cancellationToken);
            
            if (result)
                return Result<int>.Success(request.SpecialtyId);
            
            return Result<int>.Failure(["Не удалось обновить специальность"], ApiErrorType.BadRequest);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Доменная ошибка при обновлении специальности");

            return Result<int>.Failure(ex);
        }
        catch (DatabaseException ex)
        {
            _logger.LogWarning(ex, "Ошибка базы данных при обновлении специальности");

            return Result<int>.Failure(ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Непредвиденная ошибка при обновлении специальности");

            return Result<int>.Failure(ex);
        }
    }
}