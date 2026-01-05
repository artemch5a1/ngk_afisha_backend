using IdentityService.Domain.Abstractions.Application.Services.UserContext;
using IdentityService.Domain.CustomExceptions;
using IdentityService.Domain.Enums;
using IdentityService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.UseCases.CatalogCases.PostCases.DeletePost;

public class DeletePostHandler : IRequestHandler<DeletePostCommand, Result<int>>
{
    private readonly IPostService _postService;

    private readonly ILogger<DeletePostHandler> _logger;


    public DeletePostHandler(
        IPostService postService, 
        ILogger<DeletePostHandler> logger)
    {
        _postService = postService;
        _logger = logger;
    }

    public async Task<Result<int>> Handle(DeletePostCommand request, CancellationToken cancellationToken)
    {
        try
        {
            bool result = await _postService.DeletePost(request.PostId, cancellationToken);

            return result
                ? Result<int>.Success(request.PostId)
                : Result<int>.Failure(["Ошибка удаления группы"], ApiErrorType.BadRequest);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(ex, "Произошла доменная ошибка при удалении должности");
            return Result<int>.Failure(ex);
        }
        catch (DatabaseException ex)
        {
            _logger.LogWarning(ex, "Произошла ошибка базы данных при удалении должности");
            
            if(ex.ErrorType == ApiErrorType.UnprocessableEntity)
                return Result<int>.Failure(["Нельзя удалить используемую должность"], ApiErrorType.UnprocessableEntity);
            
            return Result<int>.Failure(ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Произошла непредвиденная ошибка при удалении должности");
            return Result<int>.Failure(ex);
        }
    }
}