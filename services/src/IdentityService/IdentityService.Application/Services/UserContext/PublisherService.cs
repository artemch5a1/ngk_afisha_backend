using IdentityService.Domain.Abstractions.Application.Services.UserContext;
using IdentityService.Domain.Abstractions.Infrastructure.Repositories.UserContext;
using IdentityService.Domain.Models.UserContext;

namespace IdentityService.Application.Services.UserContext;

public class PublisherService : IPublisherService
{
    private readonly IPublisherRepository _publisherRepository;

    public PublisherService(IPublisherRepository publisherRepository)
    {
        _publisherRepository = publisherRepository;
    }

    public async Task<List<Publisher>> GetAllPublisher(
        CancellationToken cancellationToken = default
    )
    {
        return await _publisherRepository.GetAll(cancellationToken);
    }

    public async Task<Publisher?> GetPublisherById(
        Guid publisherId,
        CancellationToken cancellationToken = default
    )
    {
        return await _publisherRepository.GetById(publisherId, cancellationToken);
    }
}
