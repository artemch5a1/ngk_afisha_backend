using IdentityService.Domain.Abstractions.Application.Services.UserContext;
using IdentityService.Domain.CustomExceptions;
using IdentityService.Domain.Models.UserContext;
using IdentityService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.UseCases.CatalogCases.SpecialtyCases.CreateSpecialty;

public class CreateSpecialtyHandler : IRequestHandler<CreateSpecialtyCommand, Result<Specialty>>
{
    private readonly ISpecialtyService _specialtyService;

    private readonly ILogger<CreateSpecialtyHandler> _logger;
    
    public CreateSpecialtyHandler(ISpecialtyService specialtyService, ILogger<CreateSpecialtyHandler> logger)
    {
        _specialtyService = specialtyService;
        _logger = logger;
    }
    
    public async Task<Result<Specialty>> Handle(CreateSpecialtyCommand request, CancellationToken cancellationToken)
    {
        try
        {
            Specialty result = await _specialtyService.CreateSpecialty(request.Title, cancellationToken);

            return Result<Specialty>.Success(result);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Доменная ошибка при создании специальности");

            return Result<Specialty>.Failure(ex);
        }
        catch (DatabaseException ex)
        {
            _logger.LogWarning(ex, "Ошибка базы данных при создании специальности");

            return Result<Specialty>.Failure(ex);;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Непредвиденная ошибка при создании специальности");

            return Result<Specialty>.Failure(ex);
        }
    }
}