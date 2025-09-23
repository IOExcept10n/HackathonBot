using Microsoft.EntityFrameworkCore;
using MyBots.Persistence.DTO;
using MyBots.Persistence.Repository;

namespace MyBots.Persistence.Repositories;

/// <summary>
/// Repository for managing role entities.
/// </summary>
public class RoleRepository : Repository<BasicBotDbContext, Role>
{
    public RoleRepository(BasicBotDbContext context) : base(context)
    {
    }

    /// <summary>
    /// Gets a role by its name.
    /// </summary>
    /// <param name="name">The name of the role to find.</param>
    /// <returns>The role if found; otherwise, null.</returns>
    public async Task<Role?> GetByNameAsync(string name)
    {
        return await _dbSet
            .Include(r => r.Users)
            .FirstOrDefaultAsync(r => r.Name == name);
    }

    /// <summary>
    /// Creates a new role if it doesn't exist.
    /// </summary>
    /// <param name="name">The name of the role to create.</param>
    /// <returns>The existing or newly created role.</returns>
    public async Task<Role> GetOrCreateAsync(string name)
    {
        var role = await GetByNameAsync(name);
        if (role != null)
            return role;

        role = new Role { Name = name };
        await AddAsync(role);
        await SaveChangesAsync();
        return role;
    }
}