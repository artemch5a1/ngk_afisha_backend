using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IdentityService.Domain.Abstractions.Infrastructure.Entity;
using IdentityService.Domain.Models.UserContext;

namespace IdentityService.Infrastructure.Entites.UserContext;

public class GroupEntity : IEntity<GroupEntity, Group>
{
    internal GroupEntity() { }
    
    private GroupEntity(Group group)
    {
        Course = group.Course;
        NumberGroup = group.NumberGroup;
        SpecialtyId = group.SpecialtyId;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("group_id")]
    public int GroupId { get; set; }
    
    
    [Column("course")]
    public int Course { get; set; }
    
    [Column("number_group")]
    public int NumberGroup { get; set; }
    
    [Column("specialty_id")]
    public int SpecialtyId { get; set; }

    [ForeignKey(nameof(SpecialtyId))]
    public SpecialtyEntity Specialty { get; set; } = null!;
    
    
    
    
    public Group ToDomain()
    {
        return Group.Restore(GroupId, Course, NumberGroup, SpecialtyId);
    }

    public static GroupEntity ToEntity(Group domain)
    {
        return new GroupEntity(domain);
    }
}