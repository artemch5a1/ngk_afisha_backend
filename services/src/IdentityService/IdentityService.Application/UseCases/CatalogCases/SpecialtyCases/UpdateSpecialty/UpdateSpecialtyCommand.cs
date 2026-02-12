using IdentityService.Domain.Result;
using MediatR;

namespace IdentityService.Application.UseCases.CatalogCases.SpecialtyCases.UpdateSpecialty;

public record UpdateSpecialtyCommand(int SpecialtyId, string NewSpecialtyTitle)
    : IRequest<Result<int>>;
