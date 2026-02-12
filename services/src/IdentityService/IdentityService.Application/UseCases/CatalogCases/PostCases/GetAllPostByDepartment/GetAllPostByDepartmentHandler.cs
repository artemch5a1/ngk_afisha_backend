using IdentityService.Domain.Abstractions.Application.Services.UserContext;
using IdentityService.Domain.CustomExceptions;
using IdentityService.Domain.Models.UserContext;
using IdentityService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.UseCases.CatalogCases.PostCases.GetAllPostByDepartment;

public class GetAllPostByDepartmentHandler
    : IRequestHandler<GetAllPostByDepartmentQuery, Result<List<Post>>>
{
    private readonly IPostService _postService;

    private readonly ILogger<GetAllPostByDepartmentHandler> _logger;

    public GetAllPostByDepartmentHandler(
        IPostService postService,
        ILogger<GetAllPostByDepartmentHandler> logger
    )
    {
        _postService = postService;
        _logger = logger;
    }

    public async Task<Result<List<Post>>> Handle(
        GetAllPostByDepartmentQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            List<Post> result = await _postService.GetAllPostByDepartmentId(
                request.DepartmentId,
                cancellationToken
            );

            return Result<List<Post>>.Success(result);
        }
        catch (DatabaseException ex)
        {
            _logger.LogWarning(ex, "Ошибка базы данных при получении всех должностей в отделе");

            return Result<List<Post>>.Failure(ex);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Доменная ошибка при получении всех должностей в отделе");

            return Result<List<Post>>.Failure(ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Неизвестная ошибка при получении всех должностей в отделе");

            return Result<List<Post>>.Failure(ex);
        }
    }
}
