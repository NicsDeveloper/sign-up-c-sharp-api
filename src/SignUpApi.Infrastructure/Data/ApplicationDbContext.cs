using Microsoft.EntityFrameworkCore;
using SignUpApi.Domain.Entities;
using SignUpApi.Infrastructure.Data.ValueConverters;

namespace SignUpApi.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            
            entity.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(50);
                
            entity.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(50);
                
            entity.Property(e => e.CreatedAt)
                .IsRequired();
                
            entity.Property(e => e.UpdatedAt);
                
            entity.Property(e => e.IsActive)
                .IsRequired();
                
            entity.Property(e => e.LastLoginAt);

            entity.Property(e => e.Email)
                .HasConversion(new EmailValueConverter())
                .IsRequired()
                .HasMaxLength(100);
                
            entity.Property(e => e.Password)
                .HasConversion(new PasswordValueConverter())
                .IsRequired()
                .HasMaxLength(255);

            entity.HasIndex(e => e.Email)
                .IsUnique();
        });
    }
}
