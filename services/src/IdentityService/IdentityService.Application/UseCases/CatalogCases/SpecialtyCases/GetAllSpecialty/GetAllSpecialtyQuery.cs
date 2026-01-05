using IdentityService.Domain.Models.UserContext;
using IdentityService.Domain.Result;
using MediatR;

namespace IdentityService.Application.UseCases.CatalogCases.SpecialtyCases.GetAllSpecialty;

public record GetAllSpecialtyQuery() : IRequest<Result<List<Specialty>>>;