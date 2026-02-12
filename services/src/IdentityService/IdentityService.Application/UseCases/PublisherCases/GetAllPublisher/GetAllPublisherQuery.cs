using IdentityService.Domain.Models.UserContext;
using IdentityService.Domain.Result;
using MediatR;

namespace IdentityService.Application.UseCases.PublisherCases.GetAllPublisher;

public record GetAllPublisherQuery() : IRequest<Result<List<Publisher>>>;
