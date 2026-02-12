using IdentityService.Domain.Result;
using MediatR;

namespace IdentityService.Application.UseCases.CatalogCases.SpecialtyCases.DeleteSpecialty;

public record DeleteSpecialtyCommand(int SpecialtyId) : IRequest<Result<int>>;
