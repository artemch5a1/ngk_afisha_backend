using IdentityService.Domain.Result;
using MediatR;

namespace IdentityService.Application.UseCases.CatalogCases.GroupCases.UpdateGroup;

public record UpdateGroupCommand(int GroupId, int Course, int NumberGroup, int SpecialtyId) : IRequest<Result<int>>;