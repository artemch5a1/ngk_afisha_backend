namespace IdentityService.Infrastructure.Implementations.Data.Seeding.Models;

public class PostSeed
{
    public string PostName { get; set; } = null!;

    public string DepartmentName { get; set; } = null!;
}

public class PostSeedOptions
{
    public List<PostSeed> PostSeed { get; set; } = [];
}