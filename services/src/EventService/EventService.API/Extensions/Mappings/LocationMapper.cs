using EventService.API.Contracts.Locations;
using EventService.Application.UseCases.LocationCases.CreateLocation;
using EventService.Application.UseCases.LocationCases.UpdateLocation;
using EventService.Domain.Models;

namespace EventService.API.Extensions.Mappings;

public static class LocationMapper
{
    public static LocationDto ToDto(this Location location)
    {
        return new LocationDto()
        {
            LocationId = location.LocationId,
            Title = location.Title,
            Address = location.Address,
        };
    }

    public static List<LocationDto> ToListDto(this List<Location> locations) =>
        locations.Select(x => x.ToDto()).ToList();

    public static CreateLocationCommand ToCommand(this CreateLocationDto dto)
    {
        return new CreateLocationCommand(dto.Title, dto.Address);
    }

    public static UpdateLocationCommand ToCommand(this UpdateLocationDto dto)
    {
        return new UpdateLocationCommand(dto.LocationId, dto.Title, dto.Address);
    }
}
