using IdentityService.Domain.Models.UserContext;

namespace IdentityService.Domain.Abstractions.Application.Services.UserContext;

public interface ISpecialtyService
{
    Task<List<Specialty>> GetAllSpecialty(CancellationToken cancellationToken = default);

    Task<Specialty?> GetSpecialtyById(int id, CancellationToken cancellationToken = default);

    Task<Specialty> CreateSpecialty(string title, CancellationToken cancellationToken = default);

    Task<bool> UpdateSpecialty(int specialtyId, string newSpecialtyTitle, CancellationToken cancellationToken = default);

    Task<bool> DeleteSpecialty(int specialtyId, CancellationToken cancellationToken = default);
}