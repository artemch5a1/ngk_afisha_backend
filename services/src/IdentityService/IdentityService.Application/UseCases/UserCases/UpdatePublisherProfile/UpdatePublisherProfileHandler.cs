using IdentityService.Domain.Abstractions.Application.Services.UserContext;
using IdentityService.Domain.CustomExceptions;
using IdentityService.Domain.Enums;
using IdentityService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.UseCases.UserCases.UpdatePublisherProfile;

public class UpdatePublisherProfileHandler
    : IRequestHandler<UpdatePublisherProfileCommand, Result<Guid>>
{
    private readonly IUserService _userService;

    private readonly ILogger<UpdatePublisherProfileHandler> _logger;

    public UpdatePublisherProfileHandler(
        IUserService userService,
        ILogger<UpdatePublisherProfileHandler> logger
    )
    {
        _userService = userService;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(
        UpdatePublisherProfileCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            bool result = await _userService.UpdatePublisherProfile(
                request.UserId,
                request.NewPostId
            );

            if (result)
                return Result<Guid>.Success(request.UserId);

            return Result<Guid>.Failure(["Ошибка при обновлении данных"], ApiErrorType.BadRequest);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(
                ex,
                "Доменная ошибка при обновлении профиля публикатора: {UserId}",
                request.UserId
            );
            return Result<Guid>.Failure(ex);
        }
        catch (DatabaseException ex)
        {
            _logger.LogWarning(
                ex,
                "Ошибка базы данных при обновлении профиля публикатора: {UserId}",
                request.UserId
            );

            return Result<Guid>.Failure(ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Произошла ошибка при обновлении профиля публикатора: {UserId}",
                request.UserId
            );

            return Result<Guid>.Failure(ex);
        }
    }
}
