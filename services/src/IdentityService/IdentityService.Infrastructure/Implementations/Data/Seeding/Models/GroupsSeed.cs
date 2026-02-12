namespace IdentityService.Infrastructure.Implementations.Data.Seeding.Models;

public class GroupsSeed
{
    public int Course { get; set; }

    public int Number { get; set; }

    public string SpecialtyTitle { get; set; } = null!;
}

public class GroupsSeedOption
{
    public List<GroupsSeed> GroupSeed { get; set; } = [];
};
