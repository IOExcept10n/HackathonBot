using Microsoft.EntityFrameworkCore;
using MyBots.Core.Persistence.DTO;

namespace MyBots.Core.Persistence;

/// <summary>
/// Database context for the bot system.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="BasicBotDbContext"/> class.
/// </remarks>
/// <param name="options">The options for configuring the context.</param>
public abstract class BasicBotDbContext(DbContextOptions options) : DbContext(options)
{
    /// <summary>
    /// Gets or sets the users in the system.
    /// </summary>
    public DbSet<User> Users => Set<User>();

    /// <inheritdoc />
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure roles and users
        modelBuilder.ConfigureRoleModel();
    }
}