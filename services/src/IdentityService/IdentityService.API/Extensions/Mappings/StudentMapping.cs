using IdentityService.API.Contracts.StudentActions;
using IdentityService.Domain.Models.UserContext;

namespace IdentityService.API.Extensions.Mappings;

public static class StudentMapping
{
    public static StudentDto ToDto(this Student student)
    {
        StudentDto studentDto = new StudentDto()
        {
            StudentId = student.StudentId,
            GroupId = student.GroupId
        };

        if (student.Group is not null)
        {
            studentDto.Group = student.Group.ToDto();
        }
        
        if (student.User is not null)
        {
            studentDto.User = student.User.ToDto();
        }

        return studentDto;
    }
    
    public static List<StudentDto> ToListDto(this List<Student> students)
    {
        return students.Select(x => x.ToDto()).ToList();
    }
}