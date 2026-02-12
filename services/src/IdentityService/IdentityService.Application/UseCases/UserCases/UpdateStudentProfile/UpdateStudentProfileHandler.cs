using IdentityService.Domain.Abstractions.Application.Services.UserContext;
using IdentityService.Domain.CustomExceptions;
using IdentityService.Domain.Enums;
using IdentityService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.UseCases.UserCases.UpdateStudentProfile;

public class UpdateStudentProfileHandler
    : IRequestHandler<UpdateStudentProfileCommand, Result<Guid>>
{
    private readonly IUserService _userService;

    private readonly ILogger<UpdateStudentProfileHandler> _logger;

    public UpdateStudentProfileHandler(
        IUserService userService,
        ILogger<UpdateStudentProfileHandler> logger
    )
    {
        _userService = userService;
        _logger = logger;
    }

    public async Task<Result<Guid>> Handle(
        UpdateStudentProfileCommand request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            bool result = await _userService.UpdateStudentProfile(
                request.UserId,
                request.NewGroupId
            );

            if (result)
                return Result<Guid>.Success(request.UserId);

            return Result<Guid>.Failure(["Ошибка при обновлении данных"], ApiErrorType.BadRequest);
        }
        catch (DomainException ex)
        {
            _logger.LogWarning(
                ex,
                "Доменная ошибка при обновлении профиля студента: {UserId}",
                request.UserId
            );
            return Result<Guid>.Failure(ex);
        }
        catch (DatabaseException ex)
        {
            _logger.LogWarning(
                ex,
                "Ошибка базы данных при обновлении профиля студента: {UserId}",
                request.UserId
            );

            return Result<Guid>.Failure(ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Произошла ошибка при обновлении профиля студента: {UserId}",
                request.UserId
            );

            return Result<Guid>.Failure(ex);
        }
    }
}
