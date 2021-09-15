
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Persistence {

        public class AppConfigurationDbContext : ConfigurationDbContext<AppConfigurationDbContext> {

        // Asset
        public AppConfigurationDbContext(
            DbContextOptions<AppConfigurationDbContext> options,
            ConfigurationStoreOptions  store_options)
            : base(options,store_options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) {

            base.OnModelCreating(modelBuilder);
        }
    }
}