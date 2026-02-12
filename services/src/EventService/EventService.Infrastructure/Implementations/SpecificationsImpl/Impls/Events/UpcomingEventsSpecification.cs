using System.Linq.Expressions;
using EventService.Domain.Models;
using EventService.Infrastructure.Entites;

namespace EventService.Infrastructure.Implementations.SpecificationsImpl.Impls.Events;

public class UpcomingEventsSpecification : IEfSpecificationHandler<Event, EventEntity>
{
    public Expression<Func<EventEntity, bool>> Apply() => item => item.DateStart > DateTime.UtcNow;

    public string Name => "UpcomingEvents";
}
