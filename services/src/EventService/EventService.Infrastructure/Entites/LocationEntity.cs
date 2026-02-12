using System.ComponentModel.DataAnnotations.Schema;
using EventService.Domain.Abstractions.Infrastructure.Entity;
using EventService.Domain.Models;

namespace EventService.Infrastructure.Entites;

public class LocationEntity : IEntity<LocationEntity, Location>
{
    [Column("location_id")]
    public int LocationId { get; set; }

    [Column("title")]
    public string Title { get; set; }

    [Column("address")]
    public string Address { get; set; }

    private LocationEntity(Location location)
    {
        Title = location.Title;
        Address = location.Address;
    }

    internal LocationEntity() { }

    public Location ToDomain()
    {
        return Location.Restore(LocationId, Title, Address);
    }

    public static LocationEntity ToEntity(Location domain)
    {
        return new LocationEntity(domain);
    }
}
