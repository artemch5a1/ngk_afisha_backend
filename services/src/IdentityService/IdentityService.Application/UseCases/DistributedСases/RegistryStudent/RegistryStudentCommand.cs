using IdentityService.Application.Contracts;
using IdentityService.Domain.Result;
using MediatR;

namespace IdentityService.Application.UseCases.DistributedСases.RegistryStudent;

public record RegistryStudentCommand(
    string Email, 
    string Password, 
    string Surname,
    string Name,
    string? Patronymic,
    DateOnly DateBirth,
    int GroupId
    ) : IRequest<Result<AccountCreated>>;