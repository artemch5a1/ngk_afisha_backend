using IdentityService.Domain.Models.UserContext;
using IdentityService.Domain.Result;
using MediatR;

namespace IdentityService.Application.UseCases.StudentCases.GetStudentById;

public record GetStudentByIdQuery(Guid StudentId) : IRequest<Result<Student>>;