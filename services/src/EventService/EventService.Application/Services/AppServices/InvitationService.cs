using EventService.Domain.Abstractions.Application.Services.AppServices;
using EventService.Domain.Abstractions.Infrastructure.Repositories.Repositories;
using EventService.Domain.Contract;
using EventService.Domain.Models;

namespace EventService.Application.Services.AppServices;

public class InvitationService : IInvitationService
{
    private readonly IInvitationRepository _invitationRepository;

    public InvitationService(IInvitationRepository invitationRepository)
    {
        _invitationRepository = invitationRepository;
    }

    public async Task<List<Invitation>> GetAllActualInvitation(
        PaginationContract? contract,
        CancellationToken cancellationToken = default
    )
    {
        return await _invitationRepository.GetAllActual(contract, cancellationToken);
    }

    public async Task<List<Invitation>> GetAllInvitation(
        PaginationContract? contract,
        CancellationToken cancellationToken = default
    )
    {
        return await _invitationRepository.GetAll(contract, cancellationToken);
    }

    public async Task<Invitation?> GetInvitationById(
        Guid invitationId,
        CancellationToken cancellationToken = default
    )
    {
        return await _invitationRepository.GetById(invitationId, cancellationToken);
    }

    public async Task<List<Invitation>> GetAllInvitationByAuthor(
        Guid authorId,
        PaginationContract? contract = null,
        CancellationToken cancellationToken = default
    )
    {
        return await _invitationRepository.GetAllByAuthor(authorId, contract, cancellationToken);
    }

    public async Task<List<Invitation>> GetAllInvitationByEvent(
        Guid eventId,
        PaginationContract? contract = null,
        CancellationToken cancellationToken = default
    )
    {
        return await _invitationRepository.GetAllByEvent(eventId, contract, cancellationToken);
    }
}
