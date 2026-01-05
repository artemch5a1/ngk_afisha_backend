namespace IdentityService.Infrastructure.Implementations.Data.Seeding.Models;

public class DepartmentSeed
{
    public string DepartmentName { get; set; } = null!;
}

public class DepartmentSeedOptions
{
    public List<DepartmentSeed> DepartmentSeed { get; set; } = [];
}