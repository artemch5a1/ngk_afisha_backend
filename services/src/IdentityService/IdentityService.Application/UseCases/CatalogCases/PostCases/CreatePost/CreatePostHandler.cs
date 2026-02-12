using IdentityService.Domain.Abstractions.Application.Services.UserContext;
using IdentityService.Domain.CustomExceptions;
using IdentityService.Domain.Models.UserContext;
using IdentityService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.UseCases.CatalogCases.PostCases.CreatePost;

public class CreatePostHandler : IRequestHandler<CreatePostCommand, Result<Post>>
{
    private readonly IPostService _postService;

    private readonly ILogger<CreatePostHandler> _logger;

    public CreatePostHandler(IPostService postService, ILogger<CreatePostHandler> logger)
    {
        _postService = postService;
        _logger = logger;
    }

    public async Task<Result<Post>> Handle(
        CreatePostCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            Post result = await _postService.CreatePost(
                request.DepartmentId,
                request.PostTitle,
                cancellationToken
            );

            return Result<Post>.Success(result);
        }
        catch (DatabaseException ex)
        {
            _logger.LogWarning(ex, "Ошибка базы данных при создании должности");

            return Result<Post>.Failure(ex);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Доменная ошибка при создании должности");

            return Result<Post>.Failure(ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Неизвестная ошибка при создании должности");

            return Result<Post>.Failure(ex);
        }
    }
}
