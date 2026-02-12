namespace EventService.Infrastructure.Static;

public static class PolicyNames
{
    public const string PublisherOnly = "PublisherOnly";
    public const string UserOnly = "UserOnly";
    public const string UserOrAdmin = "UserOrAdmin";
    public const string AdminOnly = "AdminOnly";
    public const string PublisherOrAdmin = "PublisherOrAdmin";
}
