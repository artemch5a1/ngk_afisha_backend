using EventService.Domain.Abstractions.Infrastructure.Storage;
using Amazon.S3;
using Amazon.S3.Model;
using Microsoft.Extensions.Options;

namespace EventService.Infrastructure.Implementations.Storage;

/// <summary>
///     Сервис работы с файловым хранилищем на базе Amazon S3 или совместимых S3-провайдеров (например, MinIO).
///     Поддерживает два клиента:
///     <list type="bullet">
///         <item>
///             <description>
///                 <b>Внешний клиент</b> — используется для генерации публичных pre-signed URL (загрузка, скачивание).
///             </description>
///         </item>
///         <item>
///             <description>
///                 <b>Внутренний (bounded) клиент</b> — используется для внутренних операций (например, удаление).
///             </description>
///         </item>
///     </list>
///     Если <see cref="S3StorageSettings.ServiceUrlBounded"/> отсутствует или совпадает с <see cref="S3StorageSettings.ServiceUrl"/>,
///     оба клиента будут совпадать.
/// </summary>
public class S3StorageService : IStorageService
{
    private readonly string _bucketName;
    private readonly IAmazonS3 _s3BoundedClient;
    private readonly IAmazonS3 _s3ForeignClient;

    /// <summary>
    ///     Создаёт экземпляр <see cref="S3StorageService"/> на основе настроек.
    /// </summary>
    /// <param name="options">Настройки подключения к S3-хранилищу.</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если настройки не указаны.</exception>
    public S3StorageService(IOptions<S3StorageSettings> options)
    {
        var settings = options.Value ?? throw new ArgumentNullException(nameof(options));

        _bucketName = settings.BucketName;

        _s3ForeignClient = CreateS3Client(settings.ServiceUrl, settings);

        _s3BoundedClient = ShouldUseSameClient(settings)
            ? _s3ForeignClient
            : CreateS3Client(settings.ServiceUrlBounded!, settings);
    }

    /// <summary>
    ///     Создаёт S3-клиент с использованием указанных настроек и конечной точки.
    /// </summary>
    /// <param name="serviceUrl">URL сервиса S3 или MinIO.</param>
    /// <param name="settings">Настройки доступа.</param>
    /// <returns>Экземпляр <see cref="IAmazonS3"/>.</returns>
    private static IAmazonS3 CreateS3Client(string serviceUrl, S3StorageSettings settings)
    {
        var config = new AmazonS3Config
        {
            ServiceURL = serviceUrl,
            ForcePathStyle = true
        };

        return new AmazonS3Client(
            settings.AccessKey,
            settings.SecretKey,
            config
        );
    }

    /// <summary>
    ///     Определяет, нужен ли отдельный bounded-клиент или можно использовать внешний.
    /// </summary>
    /// <param name="settings">Настройки подключения.</param>
    /// <returns>
    ///     <c>true</c>, если сервис должен использовать один и тот же клиент 
    ///     для внутренних операций и генерации URL; иначе <c>false</c>.
    /// </returns>
    private static bool ShouldUseSameClient(S3StorageSettings settings)
    {
        return string.IsNullOrWhiteSpace(settings.ServiceUrlBounded)
               || settings.ServiceUrlBounded == settings.ServiceUrl;
    }

    /// <summary>
    ///     Генерирует pre-signed URL для загрузки файла (HTTP PUT).
    /// </summary>
    /// <param name="key">Ключ (путь/имя файла) в бакете.</param>
    /// <param name="expiresIn">Время жизни URL.</param>
    /// <returns>Публичный URL для загрузки файла.</returns>
    public async Task<string> GenerateUploadUrlAsync(string key, TimeSpan expiresIn)
    {
        return await GeneratePreSignedUrl(key, HttpVerb.PUT, expiresIn);
    }

    /// <summary>
    ///     Генерирует pre-signed URL для скачивания файла (HTTP GET).
    /// </summary>
    /// <param name="key">Ключ (путь/имя файла) в бакете.</param>
    /// <param name="expiresIn">Время жизни URL.</param>
    /// <returns>Публичный URL для скачивания файла.</returns>
    public async Task<string> GenerateDownloadUrlAsync(string key, TimeSpan expiresIn)
    {
        return await GeneratePreSignedUrl(key, HttpVerb.GET, expiresIn);
    }

    /// <summary>
    ///     Внутренний метод генерации pre-signed URL для указанных операции (GET/PUT).
    /// </summary>
    /// <param name="key">Ключ объекта.</param>
    /// <param name="verb">HTTP-метод.</param>
    /// <param name="expiresIn">Время до истечения срока действия ссылки.</param>
    /// <returns>Сформированный pre-signed URL.</returns>
    private async Task<string> GeneratePreSignedUrl(string key, HttpVerb verb, TimeSpan expiresIn)
    {
        var request = new GetPreSignedUrlRequest
        {
            BucketName = _bucketName,
            Key = key,
            Verb = verb,
            Expires = DateTime.UtcNow.Add(expiresIn),
            Protocol = _s3ForeignClient.Config!.ServiceURL!.StartsWith("https://")
                ? Protocol.HTTPS
                : Protocol.HTTP
        };

        return await _s3ForeignClient.GetPreSignedURLAsync(request)!;
    }

    /// <summary>
    ///     Удаляет объект из хранилища.
    /// </summary>
    /// <param name="key">Ключ удаляемого объекта.</param>
    public async Task DeleteAsync(string key)
    {
        var request = new DeleteObjectRequest
        {
            BucketName = _bucketName,
            Key = key
        };

        await _s3BoundedClient.DeleteObjectAsync(request)!;
    }

    public async Task<string> UploadFileAsync(byte[] content, string fileName, string contentType,
        CancellationToken cancellationToken = default)
    {
        if (content is null || content.Length == 0)
            throw new ArgumentException("Файл пуст или отсутствует.", nameof(content));

        if (string.IsNullOrWhiteSpace(fileName))
            throw new ArgumentException("Имя файла обязательно.", nameof(fileName));

        // Имя файла = ключ в бакете
        var key = fileName.Replace("\\", "/");

        using var stream = new MemoryStream(content);

        var request = new PutObjectRequest
        {
            BucketName = _bucketName,
            Key = key,
            InputStream = stream,
            AutoCloseStream = true,
            ContentType = contentType
        };

        await _s3BoundedClient.PutObjectAsync(request, cancellationToken);

        // Формируем URL так же, как в GeneratePreSignedUrl
        // (облегчит доступ извне / из CDN / UI)
        string baseUrl = _s3ForeignClient.Config.ServiceURL!.TrimEnd('/');

        return $"{baseUrl}/{_bucketName}/{key}";
    }
}
