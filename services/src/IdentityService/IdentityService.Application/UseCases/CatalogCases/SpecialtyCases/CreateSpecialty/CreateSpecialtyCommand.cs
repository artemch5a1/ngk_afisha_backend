using IdentityService.Domain.Models.UserContext;
using IdentityService.Domain.Result;
using MediatR;

namespace IdentityService.Application.UseCases.CatalogCases.SpecialtyCases.CreateSpecialty;

public record CreateSpecialtyCommand(string Title) : IRequest<Result<Specialty>>;