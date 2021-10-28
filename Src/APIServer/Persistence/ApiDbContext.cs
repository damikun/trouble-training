using Microsoft.EntityFrameworkCore;
using APIServer.Persistence.Extensions;
using APIServer.Domain.Core.Models.Events;
using APIServer.Domain.Core.Models.WebHooks;

namespace APIServer.Persistence {

        public class ApiDbContext : DbContext {
            public DbSet<WebHook> WebHooks { get; set; }
            
            public DbSet<WebHookRecord> WebHooksHistory { get; set; }

            public DbSet<DomainEvent> Events { get; set; }

        public ApiDbContext(
            DbContextOptions<ApiDbContext> options)
            : base(options) {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            if(this.Database.IsNpgsql()){
                modelBuilder.HasPostgresExtension("citext");
            }

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApiDbContext).Assembly);

            modelBuilder.Entity<WebHookCreated>().ToTable("WebHookCreatedEvent");

            modelBuilder.Entity<WebHookRemoved>().ToTable("WebHookRemovedEvent");

            modelBuilder.Entity<WebHookUpdated>().ToTable("WebHookUpdatedEvent");

            if(!this.Database.IsNpgsql()){
                modelBuilder.Entity<WebHook>().Property(e => e.HookEvents).HasConversion(
                    new EnumArrToString_StringToEnumArr_Converter(
                        e=> EnumArrToString_StringToEnumArr_Converter.Convert(e),
                        s=> EnumArrToString_StringToEnumArr_Converter.Convert(s)
                    )
                );
            }

            //---------------------
            // Initial DB data
            //---------------------

            modelBuilder.Entity<WebHook>().HasData(
                new WebHook() { 
                    ID = 1,
                    WebHookUrl = "https://localhost:5015/hookloopback",
                    IsActive = true,
                    ContentType = "application/json",
                    HookEvents= new HookEventType[] {
                        HookEventType.hook
                    }
                },
                new WebHook() { 
                    ID = 2,
                    WebHookUrl = "https://localhost:5015/hookloopback2",
                    IsActive = false,
                    ContentType = "application/json",
                    HookEvents= new HookEventType[] {
                        HookEventType.hook
                    }
                }
                );

            base.OnModelCreating(modelBuilder);
        }
    }
}