using EventService.Domain.Abstractions.Application.Services.AppServices;
using EventService.Domain.Enums;
using EventService.Domain.Models;
using EventService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EventService.Application.UseCases.MemberUseCases.GetMemberById;

public class GetMemberByIdHandler : IRequestHandler<GetMemberByIdQuery, Result<Member>>
{
    private readonly IMemberService _memberService;
    
    private readonly ILogger<GetMemberByIdHandler> _logger;

    public GetMemberByIdHandler(IMemberService memberService, ILogger<GetMemberByIdHandler> logger)
    {
        _memberService = memberService;
        _logger = logger;
    }

    public async Task<Result<Member>> Handle(GetMemberByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            Member? member = await _memberService.GetMemberById(request.StudentId, request.InvitationId, cancellationToken);

            if (member is null)
                return Result<Member>.Failure("Участник не найден", ApiErrorType.NotFound);

            return Result<Member>.Success(member);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка при получении участников");

            return Result<Member>.Failure(ex);
        }
    }
}