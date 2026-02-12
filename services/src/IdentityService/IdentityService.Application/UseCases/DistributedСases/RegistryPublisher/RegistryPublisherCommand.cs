using IdentityService.Application.Contracts;
using IdentityService.Domain.Result;
using MediatR;

namespace IdentityService.Application.UseCases.DistributedСases.RegistryPublisher;

public record RegistryPublisherCommand(
    string Email,
    string Password,
    string Surname,
    string Name,
    string? Patronymic,
    DateOnly DateBirth,
    int PostId
) : IRequest<Result<AccountCreated>>;
