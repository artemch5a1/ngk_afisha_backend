using IdentityService.Domain.Models.UserContext;

namespace IdentityService.Domain.Abstractions.Application.Services.UserContext;

public interface IPublisherService
{
    Task<List<Publisher>> GetAllPublisher(CancellationToken cancellationToken = default);

    Task<Publisher?> GetPublisherById(
        Guid publisherId,
        CancellationToken cancellationToken = default
    );
}
