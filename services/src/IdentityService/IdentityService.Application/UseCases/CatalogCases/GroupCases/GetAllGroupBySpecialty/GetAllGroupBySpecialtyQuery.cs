using IdentityService.Domain.Models.UserContext;
using IdentityService.Domain.Result;
using MediatR;

namespace IdentityService.Application.UseCases.CatalogCases.GroupCases.GetAllGroupBySpecialty;

public record GetAllGroupBySpecialtyQuery(int SpecialtyId) : IRequest<Result<List<Group>>>;
