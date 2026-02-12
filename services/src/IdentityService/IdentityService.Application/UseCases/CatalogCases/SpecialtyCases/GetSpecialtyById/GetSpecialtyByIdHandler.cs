using IdentityService.Domain.Abstractions.Application.Services.UserContext;
using IdentityService.Domain.CustomExceptions;
using IdentityService.Domain.Enums;
using IdentityService.Domain.Models.UserContext;
using IdentityService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.UseCases.CatalogCases.SpecialtyCases.GetSpecialtyById;

public class GetSpecialtyByIdHandler : IRequestHandler<GetSpecialtyByIdQuery, Result<Specialty>>
{
    private readonly ISpecialtyService _specialtyService;

    private readonly ILogger<GetSpecialtyByIdHandler> _logger;

    public GetSpecialtyByIdHandler(
        ISpecialtyService specialtyService,
        ILogger<GetSpecialtyByIdHandler> logger
    )
    {
        _specialtyService = specialtyService;
        _logger = logger;
    }

    public async Task<Result<Specialty>> Handle(
        GetSpecialtyByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            Specialty? result = await _specialtyService.GetSpecialtyById(
                request.SpecialtyId,
                cancellationToken
            );

            if (result is null)
                return Result<Specialty>.Failure("Специальность не найдена", ApiErrorType.NotFound);

            return Result<Specialty>.Success(result);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Доменная ошибка при получении специальности по id");

            return Result<Specialty>.Failure(ex);
        }
        catch (DatabaseException ex)
        {
            _logger.LogWarning(ex, "Ошибка базы данных при получении специальности по id");

            return Result<Specialty>.Failure(ex);
            ;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Непредвиденная ошибка при получении специальности по id");

            return Result<Specialty>.Failure(ex);
        }
    }
}
