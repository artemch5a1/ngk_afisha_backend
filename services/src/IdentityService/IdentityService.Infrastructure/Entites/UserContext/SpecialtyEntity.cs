using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IdentityService.Domain.Abstractions.Infrastructure.Entity;
using IdentityService.Domain.Models.UserContext;

namespace IdentityService.Infrastructure.Entites.UserContext;

public class SpecialtyEntity : IEntity<SpecialtyEntity, Specialty>
{
    internal SpecialtyEntity() { }
    
    private SpecialtyEntity(Specialty specialty)
    {
        SpecialtyTitle = specialty.SpecialtyTitle;
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("specialty_id")]
    public int SpecialtyId { get; set; }

    [Column("specialty_title")]
    public string SpecialtyTitle { get; set; } = null!;

    public ICollection<GroupEntity> Groups { get; set; } = new List<GroupEntity>();

    public Specialty ToDomain()
    {
        return Specialty.Restore(SpecialtyId, SpecialtyTitle);
    }

    public static SpecialtyEntity ToEntity(Specialty domain)
    {
        return new SpecialtyEntity(domain);
    }
}