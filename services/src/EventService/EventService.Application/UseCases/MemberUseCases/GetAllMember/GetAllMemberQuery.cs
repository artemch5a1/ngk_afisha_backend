using EventService.Domain.Contract;
using EventService.Domain.Models;
using EventService.Domain.Result;
using MediatR;

namespace EventService.Application.UseCases.MemberUseCases.GetAllMember;

public record GetAllMemberQuery(PaginationContract? Contract) : IRequest<Result<List<Member>>>;
