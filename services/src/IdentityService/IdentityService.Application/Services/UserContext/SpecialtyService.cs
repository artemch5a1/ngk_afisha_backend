using IdentityService.Domain.Abstractions.Application.Services.UserContext;
using IdentityService.Domain.Abstractions.Infrastructure.Repositories.UserContext;
using IdentityService.Domain.CustomExceptions;
using IdentityService.Domain.Models.UserContext;

namespace IdentityService.Application.Services.UserContext;

public class SpecialtyService : ISpecialtyService
{
    private readonly ISpecialtyRepository _specialtyRepository;
    
    public SpecialtyService(ISpecialtyRepository specialtyRepository)
    {
        _specialtyRepository = specialtyRepository;
    }

    public async Task<List<Specialty>> GetAllSpecialty(CancellationToken cancellationToken = default)
    {
        return await _specialtyRepository.GetAll(cancellationToken);
    }

    public async Task<Specialty?> GetSpecialtyById(int id, CancellationToken cancellationToken = default)
    {
        return await _specialtyRepository.GetById(id, cancellationToken);
    }

    public async Task<Specialty> CreateSpecialty(string title, CancellationToken cancellationToken = default)
    {
        Specialty specialty = Specialty.CreateSpecialty(title);
        
        return await _specialtyRepository.Create(specialty, cancellationToken);
    }

    public async Task<bool> UpdateSpecialty(int specialtyId, string newSpecialtyTitle, CancellationToken cancellationToken = default)
    {
        Specialty? specialty = await _specialtyRepository.FindAsync(specialtyId, cancellationToken);

        if (specialty is null)
            throw new NotFoundException("Специальность", specialtyId);
        
        specialty.SpecialityUpdate(newSpecialtyTitle);

        return await _specialtyRepository.Update(specialty, cancellationToken);
    }

    public async Task<bool> DeleteSpecialty(int specialtyId, CancellationToken cancellationToken = default)
    {
        return await _specialtyRepository.Delete(specialtyId, cancellationToken);
    }
}