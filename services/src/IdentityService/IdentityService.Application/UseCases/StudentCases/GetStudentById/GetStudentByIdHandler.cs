using IdentityService.Domain.Abstractions.Application.Services.UserContext;
using IdentityService.Domain.Enums;
using IdentityService.Domain.Models.UserContext;
using IdentityService.Domain.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityService.Application.UseCases.StudentCases.GetStudentById;

public class GetStudentByIdHandler : IRequestHandler<GetStudentByIdQuery, Result<Student>>
{
    private readonly IStudentService _studentService;
    
    private readonly ILogger<GetStudentByIdHandler> _logger;

    public GetStudentByIdHandler(IStudentService studentService, ILogger<GetStudentByIdHandler> logger)
    {
        _studentService = studentService;
        _logger = logger;
    }

    public async Task<Result<Student>> Handle(GetStudentByIdQuery request, CancellationToken cancellationToken)
    {
        try
        {
            Student? result = await _studentService.GetStudentById(request.StudentId, cancellationToken);

            if (result is null)
                return Result<Student>.Failure(["Студент не найден"], ApiErrorType.NotFound);

            return Result<Student>.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ошибка при получении студентов");
            return Result<Student>.Failure(ex);
        }
    }
}