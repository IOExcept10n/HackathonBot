using Microsoft.EntityFrameworkCore;
using MyBots.Core.DTO;

namespace MyBots.Scheduling;

public static class PersistenceExtensions
{
    public static ModelBuilder ConfigureSchedulingModels(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ScheduledMessage>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.UserId, e.ScheduledTime });
        });
        return modelBuilder;
    }
}