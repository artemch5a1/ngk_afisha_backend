using System.ComponentModel.DataAnnotations.Schema;
using IdentityService.Domain.Abstractions.Infrastructure.Entity;
using IdentityService.Domain.Models.UserContext;

namespace IdentityService.Infrastructure.Entites.UserContext;

public class UserEntity : IEntity<UserEntity, User>
{
    [Column("user_id")]
    public Guid UserId { get; set; }

    [Column("surname")]
    public string Surname { get; set; } = null!;

    [Column("name")]
    public string Name { get; set; } = null!;

    [Column("patronymic")]
    public string? Patronymic { get; set; }

    [Column("birth_date")]
    public DateOnly BirthDate { get; set; }

    public StudentEntity? StudentProfile { get; set; }

    public PublisherEntity? PublisherProfile { get; set; }

    internal UserEntity() { }

    private UserEntity(User user)
    {
        UserId = user.UserId;
        Name = user.Name;
        Surname = user.Surname;
        Patronymic = user.Patronymic;
        BirthDate = user.BirthDate;

        if (user.StudentProfile is not null)
            StudentProfile = StudentEntity.ToEntity(user.StudentProfile);

        if (user.PublisherProfile is not null)
            PublisherProfile = PublisherEntity.ToEntity(user.PublisherProfile);
    }

    public User ToDomain()
    {
        return User.Restore(UserId, Surname, Name, Patronymic, BirthDate);
    }

    public static UserEntity ToEntity(User domain)
    {
        return new UserEntity(domain);
    }
}
