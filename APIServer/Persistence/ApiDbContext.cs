

using Microsoft.EntityFrameworkCore;
using APIServer.Domain.Core.Models.WebHooks;

namespace APIServer.Persistence {

        public class ApiDbContext : DbContext {
            public DbSet<WebHook> WebHooks { get; set; }
            public DbSet<WebHookRecord> WebHooksHistory { get; set; }
        // Asset
        public ApiDbContext(
            DbContextOptions<ApiDbContext> options)
            : base(options) {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.HasPostgresExtension("citext");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApiDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}