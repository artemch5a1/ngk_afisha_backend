using EntityFramework.Exceptions.PostgreSQL;
using EventService.Infrastructure.Entites;
using Microsoft.EntityFrameworkCore;

namespace EventService.Infrastructure.Data.Database;

public class EventServiceDbContext : DbContext
{
    public EventServiceDbContext(DbContextOptions<EventServiceDbContext> options) : base(options)
    {
        
    }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseExceptionProcessor();
    }
    
    public DbSet<EventEntity> Events { get; set; }

    public DbSet<LocationEntity> Locations { get; set; }

    public DbSet<GenreEntity> Genres { get; set; }

    public DbSet<EventTypeEntity> EventTypes { get; set; }

    public DbSet<InvitationEntity> Invitations { get; set; }

    public DbSet<EventRoleEntity> EventRoles { get; set; }

    public DbSet<MemberEntity> Members { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<EventEntity>(entity =>
        {
            entity.ToTable("events");

            entity.HasKey(e => e.EventId);

            entity.HasOne(e => e.Location)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(e => e.Type)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
            
            entity.HasOne(e => e.Genre)
                .WithMany()
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<LocationEntity>(entity =>
        {
            entity.ToTable("locations");

            entity.HasKey(e => e.LocationId);
        });

        modelBuilder.Entity<EventTypeEntity>(entity =>
        {
            entity.ToTable("event_types");

            entity.HasKey(e => e.TypeId);
        });
        
        modelBuilder.Entity<GenreEntity>(entity =>
        {
            entity.ToTable("genres");

            entity.HasKey(e => e.GenreId);
        });

        modelBuilder.Entity<EventRoleEntity>(entity =>
        {
            entity.ToTable("event_roles");
            
            entity.HasKey(e => e.EventRoleId);
        });

        modelBuilder.Entity<InvitationEntity>(entity =>
        {
            entity.ToTable("invitations");

            entity.HasKey(e => e.InvitationId);

            entity.HasOne(e => e.Event)
                .WithMany(e => e.Invitations)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Role)
                .WithMany()
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(e => new { e.RoleId, e.EventId }).IsUnique();
        });

        modelBuilder.Entity<MemberEntity>(entity =>
        {
            entity.ToTable("members");

            entity.HasKey(e => new { e.InvitationId, e.StudentId });

            entity.HasOne(e => e.Invitation)
                .WithMany(e => e.Members)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}