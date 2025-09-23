using Microsoft.EntityFrameworkCore;
using MyBots.Persistence.DTO;

namespace MyBots.Persistence;

/// <summary>
/// Database context for the bot system.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="BasicBotDbContext"/> class.
/// </remarks>
/// <param name="options">The options for configuring the context.</param>
public abstract class BasicBotDbContext(DbContextOptions<BasicBotDbContext> options) : DbContext(options)
{
    /// <summary>
    /// Gets or sets the users in the system.
    /// </summary>
    public DbSet<User> Users => Set<User>();

    /// <summary>
    /// Gets or sets the roles in the system.
    /// </summary>
    public DbSet<Role> Roles => Set<Role>();

    /// <summary>
    /// Gets or sets the audit log entries.
    /// </summary>
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure roles and users
        modelBuilder.ConfigureRoleModel();
    }
}