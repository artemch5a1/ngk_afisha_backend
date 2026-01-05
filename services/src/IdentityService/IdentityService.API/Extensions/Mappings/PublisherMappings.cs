using IdentityService.API.Contracts.PublisherActions;
using IdentityService.Domain.Models.UserContext;

namespace IdentityService.API.Extensions.Mappings;

public static class PublisherMappings
{
    public static PublisherDto ToDto(this Publisher publisher)
    {
        PublisherDto publisherDto = new PublisherDto()
        {
            PublisherId = publisher.PublisherId,
            PostId = publisher.PostId,
        };

        if (publisher.Post is not null)
            publisherDto.Post = publisher.Post.ToDto();

        if (publisher.User is not null)
            publisherDto.User = publisher.User.ToDto();

        return publisherDto;
    }

    public static List<PublisherDto> ToListDto(this List<Publisher> publishers)
    {
        return publishers.Select(x => x.ToDto()).ToList();
    }
}