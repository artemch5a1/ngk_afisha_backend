using EventService.Application.Settings.Events;
using EventService.Domain.Abstractions.Application.Services.AppServices;
using EventService.Domain.Abstractions.Infrastructure.Storage;
using EventService.Domain.Models;
using EventService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EventService.Application.UseCases.MemberUseCases.GetAllMemberByAuthor;

public class GetAllMemberByAuthorHandler : IRequestHandler<GetAllMemberByAuthorQuery, Result<List<Member>>>
{
    private readonly IMemberService _memberService;
    
    private readonly ILogger<GetAllMemberByAuthorHandler> _logger;
    private readonly IStorageService _storageService;
    private readonly EventSetting _eventSetting;

    public GetAllMemberByAuthorHandler(IMemberService memberService, ILogger<GetAllMemberByAuthorHandler> logger, 
        IStorageService storageService, 
        IOptions<EventSetting> eventSetting)
    {
        _memberService = memberService;
        _logger = logger;
        _storageService = storageService;
        _eventSetting = eventSetting.Value;
    }

    public async Task<Result<List<Member>>> Handle(GetAllMemberByAuthorQuery request, CancellationToken cancellationToken)
    {
        try
        {
            List<Member> members = await _memberService.GetAllMemberByAuthor(request.AuthorId, request.Contract, cancellationToken);

            foreach (Event @event in members.Select(i => i.Invitation.Event))
            {
                string url = await _storageService
                    .GenerateDownloadUrlAsync(@event.PreviewUrl, 
                        TimeSpan.FromMinutes(_eventSetting.TimeActiveDownloadLinkInMilliSeconds));
                
                @event.SetDownloadUrl(url);
            }
            
            return Result<List<Member>>.Success(members);
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Ошибка при получении участников");

            return Result<List<Member>>.Failure(ex);
        }
    }
}