using IdentityService.Domain.Abstractions.Application.Services.StartupService;

namespace IdentityService.Infrastructure.Implementations.Data.Seeding.Models;

public class SpecialtySeed
{
    public string SpecialtyTitle { get; set; } = null!;
}

public class SpecialtySeedOption
{
    public List<SpecialtySeed> SpecialtySeed { get; set; } = [];
};