using EventService.Domain.Abstractions.Application.Services.AppServices;
using EventService.Domain.Abstractions.Infrastructure.Repositories.Repositories;
using EventService.Domain.Contract;
using EventService.Domain.Models;

namespace EventService.Application.Services.AppServices;

public class MemberService : IMemberService
{
    private readonly IMemberRepository _memberRepository;

    public MemberService(IMemberRepository memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<List<Member>> GetAllMember(PaginationContract? contract = null, CancellationToken cancellationToken = default)
    {
        return await _memberRepository.GetAll(contract, cancellationToken);
    }

    public async Task<List<Member>> GetAllMemberByAuthor(Guid authorId, PaginationContract? contract = null,
        CancellationToken cancellationToken = default)
    {
        return await _memberRepository.GetAllByAuthor(authorId, contract, cancellationToken);
    }

    public async Task<List<Member>> GetAllMemberByStudent(Guid studentId, PaginationContract? contract = null,
        CancellationToken cancellationToken = default)
    {
        return await _memberRepository.GetAllByStudent(studentId, contract, cancellationToken);
    }

    public async Task<Member?> GetMemberById(Guid studentId, Guid invitationId, CancellationToken cancellationToken = default)
    {
        return await _memberRepository.GetById((invitationId, studentId), cancellationToken);
    }
}