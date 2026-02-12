namespace EventService.API.Contracts.Events;

public class UpdatedEventDto
{
    public Guid EventId { get; set; }

    public string UploadUrl { get; set; } = null!;
}
