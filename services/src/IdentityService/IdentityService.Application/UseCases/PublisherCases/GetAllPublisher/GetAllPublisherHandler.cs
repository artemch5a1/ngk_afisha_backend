using IdentityService.Domain.Abstractions.Application.Services.UserContext;
using IdentityService.Domain.Models.UserContext;
using IdentityService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.UseCases.PublisherCases.GetAllPublisher;

public class GetAllPublisherHandler : IRequestHandler<GetAllPublisherQuery, Result<List<Publisher>>>
{
    private readonly IPublisherService _publisherService;

    private readonly ILogger<GetAllPublisherHandler> _logger;
    
    public GetAllPublisherHandler(IPublisherService publisherService, ILogger<GetAllPublisherHandler> logger)
    {
        _publisherService = publisherService;
        _logger = logger;
    }

    public async Task<Result<List<Publisher>>> Handle(GetAllPublisherQuery request, CancellationToken cancellationToken)
    {
        try
        {
            List<Publisher> result = await _publisherService.GetAllPublisher(cancellationToken);

            return Result<List<Publisher>>.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении публикаторов");
            return Result<List<Publisher>>.Failure(ex);
        }
    }
}