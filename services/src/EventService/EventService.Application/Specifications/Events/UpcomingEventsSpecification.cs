using EventService.Domain.Abstractions.Specification;
using EventService.Domain.Models;

namespace EventService.Application.Specifications.Events;

public class UpcomingEventsSpecification : ISpecification<Event>
{
    public string Name => "UpcomingEvents";

    public bool IsSatisfiedBy(Event item) => item.DateStart > DateTime.UtcNow;
}
