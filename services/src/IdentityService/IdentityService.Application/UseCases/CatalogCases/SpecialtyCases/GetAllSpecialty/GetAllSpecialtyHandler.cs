using IdentityService.Domain.Abstractions.Application.Services.UserContext;
using IdentityService.Domain.CustomExceptions;
using IdentityService.Domain.Models.UserContext;
using IdentityService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.UseCases.CatalogCases.SpecialtyCases.GetAllSpecialty;

public class GetAllSpecialtyHandler : IRequestHandler<GetAllSpecialtyQuery, Result<List<Specialty>>>
{
    private readonly ISpecialtyService _specialtyService;

    private readonly ILogger<GetAllSpecialtyHandler> _logger;

    public GetAllSpecialtyHandler(
        ISpecialtyService specialtyService,
        ILogger<GetAllSpecialtyHandler> logger
    )
    {
        _specialtyService = specialtyService;
        _logger = logger;
    }

    public async Task<Result<List<Specialty>>> Handle(
        GetAllSpecialtyQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            List<Specialty> result = await _specialtyService.GetAllSpecialty(cancellationToken);

            return Result<List<Specialty>>.Success(result);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Доменная ошибка при получении всех специальностей");

            return Result<List<Specialty>>.Failure(ex);
        }
        catch (DatabaseException ex)
        {
            _logger.LogWarning(ex, "Ошибка базы данных при получении всех специальностей");

            return Result<List<Specialty>>.Failure(ex);
            ;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Непредвиденная ошибка при получении всех специальностей");

            return Result<List<Specialty>>.Failure(ex);
        }
    }
}
