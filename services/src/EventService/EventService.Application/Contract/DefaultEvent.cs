namespace EventService.Application.Contract;

public class DefaultEvent
{
    public string Title { get; set; } = null!;

    public string ShortDescription { get; set; } = null!;

    public string Description { get; set; } = null!;

    public int LocationId { get; set; }

    public int GenreId { get; set; }

    public int TypeId { get; set; }

    public int MinAge { get; set; }

    public string ImagePath { get; set; } = null!;
}

public class DefaultEventOptions
{
    public List<DefaultEvent> DefaultEvents { get; set; } = [];
}
