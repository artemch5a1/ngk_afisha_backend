using EventService.Domain.Contract;
using EventService.Domain.Models;
using EventService.Domain.Result;
using MediatR;

namespace EventService.Application.UseCases.MemberUseCases.GetAllMemberByStudent;

public record GetAllMemberByStudentQuery(Guid StudentId, PaginationContract? Contract)
    : IRequest<Result<List<Member>>>;
