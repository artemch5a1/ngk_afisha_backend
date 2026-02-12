using IdentityService.Domain.Abstractions.Application.Services.UserContext;
using IdentityService.Domain.Enums;
using IdentityService.Domain.Models.UserContext;
using IdentityService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.UseCases.PublisherCases.GetPublisherById;

public class GetPublisherByIdHandler : IRequestHandler<GetPublisherByIdQuery, Result<Publisher>>
{
    private readonly IPublisherService _publisherService;

    private readonly ILogger<GetPublisherByIdHandler> _logger;

    public GetPublisherByIdHandler(
        IPublisherService publisherService,
        ILogger<GetPublisherByIdHandler> logger
    )
    {
        _publisherService = publisherService;
        _logger = logger;
    }

    public async Task<Result<Publisher>> Handle(
        GetPublisherByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        try
        {
            Publisher? result = await _publisherService.GetPublisherById(
                request.PublisherId,
                cancellationToken
            );

            if (result is null)
                return Result<Publisher>.Failure(["Публикатор не найден"], ApiErrorType.NotFound);

            return Result<Publisher>.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении публикатора");
            return Result<Publisher>.Failure(ex);
        }
    }
}
