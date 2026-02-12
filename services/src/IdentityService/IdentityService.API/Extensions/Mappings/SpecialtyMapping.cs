using IdentityService.API.Contracts.SpecialtyActions;
using IdentityService.Application.UseCases.CatalogCases.SpecialtyCases.CreateSpecialty;
using IdentityService.Application.UseCases.CatalogCases.SpecialtyCases.UpdateSpecialty;
using IdentityService.Domain.Models.UserContext;

namespace IdentityService.API.Extensions.Mappings;

public static class SpecialtyMapping
{
    public static SpecialtyDto ToDto(this Specialty specialty)
    {
        return new SpecialtyDto()
        {
            SpecialtyId = specialty.SpecialtyId,
            SpecialtyTitle = specialty.SpecialtyTitle,
        };
    }

    public static List<SpecialtyDto> ToListDto(this List<Specialty> specialty)
    {
        return specialty.Select(x => x.ToDto()).ToList();
    }

    public static CreateSpecialtyCommand ToCommand(this CreateSpecialtyDto specialtyDto)
    {
        return new CreateSpecialtyCommand(specialtyDto.SpecialtyTitle);
    }

    public static UpdateSpecialtyCommand ToCommand(this UpdateSpecialtyDto specialtyDto)
    {
        return new UpdateSpecialtyCommand(specialtyDto.SpecialtyId, specialtyDto.SpecialtyTitle);
    }
}
