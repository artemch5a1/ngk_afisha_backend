using IdentityService.Domain.Models.UserContext;
using IdentityService.Domain.Result;
using MediatR;

namespace IdentityService.Application.UseCases.PublisherCases.GetPublisherById;

public record GetPublisherByIdQuery(Guid PublisherId) : IRequest<Result<Publisher>>;
