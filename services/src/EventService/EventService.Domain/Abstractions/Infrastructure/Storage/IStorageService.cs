namespace EventService.Domain.Abstractions.Infrastructure.Storage;

public interface IStorageService
{
    Task<string> GenerateUploadUrlAsync(string key, TimeSpan expiresIn);

    Task<string> GenerateDownloadUrlAsync(string key, TimeSpan expiresIn);

    Task DeleteAsync(string key);
    
    Task<string> UploadFileAsync(byte[] content, string fileName, string contentType, CancellationToken cancellationToken = default);
}