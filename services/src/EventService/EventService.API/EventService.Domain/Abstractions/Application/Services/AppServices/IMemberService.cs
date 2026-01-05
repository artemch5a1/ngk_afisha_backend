using EventService.Domain.Contract;
using EventService.Domain.Models;

namespace EventService.Domain.Abstractions.Application.Services.AppServices;

public interface IMemberService
{
    Task<List<Member>> GetAllMember(PaginationContract? contract = null, CancellationToken cancellationToken = default);
    
    Task<List<Member>> GetAllMemberByAuthor(
        Guid authorId, 
        PaginationContract? contract = null, 
        CancellationToken cancellationToken = default);
    
    Task<List<Member>> GetAllMemberByStudent(
        Guid studentId, 
        PaginationContract? contract = null, 
        CancellationToken cancellationToken = default);

    Task<Member?> GetMemberById(
        Guid studentId, 
        Guid invitationId, 
        CancellationToken cancellationToken = default);
}