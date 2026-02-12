using EventService.Domain.Models;

namespace EventService.Application.Contract;

public class UpdatedEvent
{
    public Guid EventId { get; set; }

    public string UploadUrl { get; set; } = null!;

    public UpdatedEvent(Event @event, string uploadUrl)
    {
        EventId = @event.EventId;
        UploadUrl = uploadUrl;
    }
}
