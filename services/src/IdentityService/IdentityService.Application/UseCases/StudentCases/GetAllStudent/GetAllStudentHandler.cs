using IdentityService.Domain.Abstractions.Application.Services.UserContext;
using IdentityService.Domain.Models.UserContext;
using IdentityService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.UseCases.StudentCases.GetAllStudent;

public class GetAllStudentHandler : IRequestHandler<GetAllStudentQuery, Result<List<Student>>>
{
    private readonly IStudentService _studentService;

    private readonly ILogger<GetAllStudentHandler> _logger;

    public GetAllStudentHandler(IStudentService studentService, ILogger<GetAllStudentHandler> logger)
    {
        _studentService = studentService;
        _logger = logger;
    }

    public async Task<Result<List<Student>>> Handle(GetAllStudentQuery request, CancellationToken cancellationToken)
    {
        try
        {
            List<Student> result = await _studentService.GetAllStudent(cancellationToken);

            return Result<List<Student>>.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении студентов");
            return Result<List<Student>>.Failure(ex);
        }
    }
}