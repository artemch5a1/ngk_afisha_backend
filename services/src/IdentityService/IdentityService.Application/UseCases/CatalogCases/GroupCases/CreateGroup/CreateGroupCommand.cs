using IdentityService.Domain.Models.UserContext;
using IdentityService.Domain.Result;
using MediatR;

namespace IdentityService.Application.UseCases.CatalogCases.GroupCases.CreateGroup;

public record CreateGroupCommand(int Course, int NumberGroup, int SpecialtyId)
    : IRequest<Result<Group>>;
