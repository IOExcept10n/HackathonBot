using Microsoft.EntityFrameworkCore;

namespace MyBots.Core.Persistence.DTO;

/// <summary>
/// Provides extension methods for configuring Entity Framework Core models.
/// </summary>
public static class PersistenceExtensions
{
    /// <summary>
    /// Configures the Entity Framework Core model for entities.
    /// </summary>
    /// <remarks>
    /// This method sets up:
    /// - Primary keys for all entities
    /// - A unique index on User.TelegramId
    /// </remarks>
    /// <param name="modelBuilder">The model builder to configure.</param>
    /// <returns>The configured model builder for method chaining.</returns>
    public static ModelBuilder ConfigureRoleModel(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.TelegramId).IsUnique();
        });

        return modelBuilder;
    }
}
