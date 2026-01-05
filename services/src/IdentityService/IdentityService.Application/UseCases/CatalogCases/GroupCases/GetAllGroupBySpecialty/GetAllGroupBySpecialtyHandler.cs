using IdentityService.Domain.Abstractions.Application.Services.UserContext;
using IdentityService.Domain.CustomExceptions;
using IdentityService.Domain.Models.UserContext;
using IdentityService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.UseCases.CatalogCases.GroupCases.GetAllGroupBySpecialty;

public class GetAllGroupBySpecialtyHandler : IRequestHandler<GetAllGroupBySpecialtyQuery, Result<List<Group>>>
{
    private readonly IGroupService _groupService;

    private readonly ILogger<GetAllGroupBySpecialtyHandler> _logger;
    
    public GetAllGroupBySpecialtyHandler(IGroupService groupService, ILogger<GetAllGroupBySpecialtyHandler> logger)
    {
        _groupService = groupService;
        _logger = logger;
    }

    public async Task<Result<List<Group>>> Handle(GetAllGroupBySpecialtyQuery request, CancellationToken cancellationToken)
    {
        try
        {
            List<Group> result = await _groupService.GetAllGroupSpecialtyId(request.SpecialtyId, cancellationToken);

            return Result<List<Group>>.Success(result);
        }
        catch (DatabaseException ex)
        {
            _logger.LogWarning(ex, "Ошибка базы данных при получении всех групп по специальности");

            return Result<List<Group>>.Failure(ex);;
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Доменная ошибка при получении всех групп по специальности");

            return Result<List<Group>>.Failure(ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Неизвестная ошибка при получении всех групп по специальности");

            return Result<List<Group>>.Failure(ex);
        }
    }
}