using IdentityService.Domain.Models.UserContext;
using IdentityService.Domain.Result;
using MediatR;

namespace IdentityService.Application.UseCases.CatalogCases.PostCases.GetAllPost;

public record GetAllPostQuery() : IRequest<Result<List<Post>>>;