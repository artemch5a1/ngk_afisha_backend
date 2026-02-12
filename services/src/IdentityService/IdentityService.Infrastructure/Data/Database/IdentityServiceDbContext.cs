using EntityFramework.Exceptions.PostgreSQL;
using IdentityService.Domain.Models.UserContext;
using IdentityService.Infrastructure.Entites.AccountContext;
using IdentityService.Infrastructure.Entites.UserContext;
using IdentityService.Infrastructure.Static;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Data.Database;

public class IdentityServiceDbContext : DbContext
{
    public IdentityServiceDbContext(DbContextOptions<IdentityServiceDbContext> options)
        : base(options) { }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseExceptionProcessor();
    }

    //AccountContext
    public DbSet<AccountEntity> Accounts { get; set; }

    //UserContext
    public DbSet<UserEntity> Users { get; set; }

    public DbSet<SpecialtyEntity> Specialties { get; set; }

    public DbSet<GroupEntity> Groups { get; set; }

    public DbSet<StudentEntity> Students { get; set; }

    public DbSet<DepartmentEntity> Departments { get; set; }

    public DbSet<PostEntity> Posts { get; set; }

    public DbSet<PublisherEntity> Publishers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Контекст AccountService

        modelBuilder.Entity<AccountEntity>(entity =>
        {
            entity.ToTable("accounts", Schemes.AccountScheme);
            entity.HasKey(e => e.AccountId);

            entity.HasIndex(e => e.Email).IsUnique();
        });

        // Контекст UserService

        modelBuilder.Entity<UserEntity>(entity =>
        {
            entity.ToTable("users", Schemes.UserScheme);
            entity.HasKey(e => e.UserId);
        });

        modelBuilder.Entity<StudentEntity>(entity =>
        {
            entity.ToTable("students", Schemes.UserScheme);

            entity.HasKey(e => e.StudentId);

            entity
                .HasOne(e => e.User)
                .WithOne(e => e.StudentProfile)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Group).WithMany().OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<SpecialtyEntity>(entity =>
        {
            entity.ToTable("specialties", Schemes.UserScheme);

            entity.HasKey(e => e.SpecialtyId);
        });

        modelBuilder.Entity<GroupEntity>(entity =>
        {
            entity.ToTable("groups", Schemes.UserScheme);

            entity.HasKey(e => e.GroupId);

            entity
                .HasOne(e => e.Specialty)
                .WithMany(r => r.Groups)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<DepartmentEntity>(entity =>
        {
            entity.ToTable("departments", Schemes.UserScheme);

            entity.HasKey(e => e.DepartmentId);
        });

        modelBuilder.Entity<PostEntity>(entity =>
        {
            entity.ToTable("posts", Schemes.UserScheme);

            entity.HasKey(e => e.PostId);

            entity
                .HasOne(e => e.Department)
                .WithMany(e => e.Posts)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<PublisherEntity>(entity =>
        {
            entity.ToTable("publishers", Schemes.UserScheme);

            entity.HasKey(e => e.PublisherId);

            entity.HasOne(e => e.User).WithOne(e => e.PublisherProfile);

            entity.HasOne(e => e.Post).WithMany().OnDelete(DeleteBehavior.Restrict);
        });
    }
}
