using IdentityService.Domain.Result;
using MediatR;

namespace IdentityService.Application.UseCases.UserCases.UpdateUser;

public record UpdateUserCommand(
    Guid UserId, 
    string Surname, 
    string Name, 
    string? Patronymic, 
    DateOnly DateBirth
    ) : IRequest<Result<Guid>>;