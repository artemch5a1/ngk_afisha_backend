using IdentityService.Domain.Result;
using MediatR;

namespace IdentityService.Application.UseCases.UserCases.UpdatePublisherProfile;

public record UpdatePublisherProfileCommand(Guid UserId, int NewPostId) : IRequest<Result<Guid>>;