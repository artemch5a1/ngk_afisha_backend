using IdentityService.Domain.Result;
using MediatR;

namespace IdentityService.Application.UseCases.UserCases.UpdateStudentProfile;

public record UpdateStudentProfileCommand(Guid UserId, int NewGroupId) : IRequest<Result<Guid>>;