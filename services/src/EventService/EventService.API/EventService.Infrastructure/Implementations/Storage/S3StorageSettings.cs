namespace EventService.Infrastructure.Implementations.Storage;

public class S3StorageSettings
{
    public string BucketName = null!;

    public string AccessKey = null!;

    public string SecretKey = null!;

    public string ServiceUrl = null!;

    public string? ServiceUrlBounded = null;
}