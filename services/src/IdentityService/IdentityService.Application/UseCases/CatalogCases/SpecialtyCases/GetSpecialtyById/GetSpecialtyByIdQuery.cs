using IdentityService.Domain.Models.UserContext;
using IdentityService.Domain.Result;
using MediatR;

namespace IdentityService.Application.UseCases.CatalogCases.SpecialtyCases.GetSpecialtyById;

public record GetSpecialtyByIdQuery(int SpecialtyId) : IRequest<Result<Specialty>>;
