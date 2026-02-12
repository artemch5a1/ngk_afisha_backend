using IdentityService.Domain.Abstractions.Application.Services.UserContext;
using IdentityService.Domain.CustomExceptions;
using IdentityService.Domain.Enums;
using IdentityService.Domain.Models.UserContext;
using IdentityService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.UseCases.CatalogCases.PostCases.GetPostById;

public class GetPostByIdHandler : IRequestHandler<GetPostByIdQuery, Result<Post>>
{
    private readonly IPostService _postService;

    private readonly ILogger<GetPostByIdHandler> _logger;

    public GetPostByIdHandler(IPostService postService, ILogger<GetPostByIdHandler> logger)
    {
        _postService = postService;
        _logger = logger;
    }

    public async Task<Result<Post>> Handle(
        GetPostByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            Post? result = await _postService.GetPostById(request.PostId, cancellationToken);

            if (result is null)
                return Result<Post>.Failure("Должность не найдена", ApiErrorType.NotFound);

            return Result<Post>.Success(result);
        }
        catch (DatabaseException ex)
        {
            _logger.LogWarning(ex, "Ошибка базы данных при получении должности по id");

            return Result<Post>.Failure(ex);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Доменная ошибка при получении должности по id");

            return Result<Post>.Failure(ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Неизвестная ошибка при получении должности по id");

            return Result<Post>.Failure(ex);
        }
    }
}
