using Microsoft.EntityFrameworkCore;

namespace MyBots.Persistence.DTO;

public static class PersistenceExtensions
{
    public static ModelBuilder ConfigureRoleModel(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.TelegramId).IsUnique();
            entity.HasMany(e => e.Roles)
                .WithMany(e => e.Users);
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
