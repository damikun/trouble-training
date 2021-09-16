

using Microsoft.EntityFrameworkCore;
using APIServer.Domain.Core.Models.WebHooks;
using APIServer.Domain.Core.Models.Events;

namespace APIServer.Persistence {

        public class ApiDbContext : DbContext {
            public DbSet<WebHook> WebHooks { get; set; }
            public DbSet<WebHookRecord> WebHooksHistory { get; set; }

            public DbSet<DomainEvent> Events { get; set; }
        // Asset
        public ApiDbContext(
            DbContextOptions<ApiDbContext> options)
            : base(options) {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            modelBuilder.HasPostgresExtension("citext");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApiDbContext).Assembly);

            modelBuilder.Entity<WebHookCreated>().ToTable("WebHookCreatedEvent");

            modelBuilder.Entity<WebHookRemoved>().ToTable("WebHookRemovedEvent");

            modelBuilder.Entity<WebHookUpdated>().ToTable("WebHookUpdatedEvent");


            base.OnModelCreating(modelBuilder);
        }
    }
}