using IdentityService.API.Contracts.PostActions;
using IdentityService.API.Contracts.UserActions;

namespace IdentityService.API.Contracts.PublisherActions;

public class PublisherDto
{
    public Guid PublisherId { get; set; }

    public UserDto User { get; set; } = null!;

    public int PostId { get; set; }

    public PostDto Post { get; set; } = null!;
}
