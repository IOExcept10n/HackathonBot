using Microsoft.EntityFrameworkCore;
using MyBots.Persistence;

namespace MyBots.Core.Fsm.Persistency
{
    public abstract class FsmDbContextBase(DbContextOptions<BasicBotDbContext> options) : BasicBotDbContext(options)
    {
        public DbSet<SessionState> Sessions => Set<SessionState>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<SessionState>(entity =>
            {
                entity.HasKey(e => e.UserId);
            });
        }
    }
}