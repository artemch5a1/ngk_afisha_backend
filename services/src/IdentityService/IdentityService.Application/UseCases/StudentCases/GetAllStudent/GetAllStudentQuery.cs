using IdentityService.Domain.Models.UserContext;
using IdentityService.Domain.Result;
using MediatR;

namespace IdentityService.Application.UseCases.StudentCases.GetAllStudent;

public record GetAllStudentQuery() : IRequest<Result<List<Student>>>;