using IdentityService.Domain.Abstractions.Application.Services.UserContext;
using IdentityService.Domain.CustomExceptions;
using IdentityService.Domain.Enums;
using IdentityService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.UseCases.CatalogCases.PostCases.UpdatePost;

public class UpdatePostHandler : IRequestHandler<UpdatePostCommand, Result<int>>
{
    private readonly IPostService _postService;

    private readonly ILogger<UpdatePostHandler> _logger;


    public UpdatePostHandler(
        IPostService postService, 
        ILogger<UpdatePostHandler> logger)
    {
        _postService = postService;
        _logger = logger;
    }

    public async Task<Result<int>> Handle(UpdatePostCommand request, CancellationToken cancellationToken)
    {
        try
        {
            bool result = await _postService.UpdatePost(request.PostId, request.PostTitle,  request.DepartmentId,cancellationToken);

            return result
                ? Result<int>.Success(request.PostId)
                : Result<int>.Failure(["Ошибка обновления группы"], ApiErrorType.BadRequest);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Произошла доменная ошибка при обновлении должности");
            return Result<int>.Failure(ex);
        }
        catch (DatabaseException ex)
        {
            _logger.LogWarning(ex, "Произошла ошибка базы данных при обновлении должности");
            return Result<int>.Failure(ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Произошла непредвиденная ошибка при обновлении должности");
            return Result<int>.Failure(ex);
        }
    }
}