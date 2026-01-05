using IdentityService.Domain.Result;
using MediatR;

namespace IdentityService.Application.UseCases.CatalogCases.GroupCases.DeleteGroup;

public record DeleteGroupCommand(int GroupId) : IRequest<Result<int>>;