using Microsoft.EntityFrameworkCore;

namespace MyBots.Persistence.DTO;

/// <summary>
/// Provides extension methods for configuring Entity Framework Core models.
/// </summary>
public static class PersistenceExtensions
{
    /// <summary>
    /// Configures the Entity Framework Core model for User, Role, and AuditLog entities.
    /// </summary>
    /// <remarks>
    /// This method sets up:
    /// - Primary keys for all entities
    /// - A unique index on User.TelegramId
    /// - A unique index on Role.Name
    /// - Relationships between entities (User-Role, AuditLog-User)
    /// </remarks>
    /// <param name="modelBuilder">The model builder to configure.</param>
    /// <returns>The configured model builder for method chaining.</returns>
    public static ModelBuilder ConfigureRoleModel(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.TelegramId).IsUnique();
            entity.HasOne(e => e.Role)
                .WithMany(e => e.Users)
                .HasForeignKey(e => e.RoleId);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Name).IsUnique();
        });

        modelBuilder.Entity<AuditLog>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId);
        });
        return modelBuilder;
    }
}
