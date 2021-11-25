
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Persistence
{

    public class AppPersistedGrantDbContext : PersistedGrantDbContext<AppPersistedGrantDbContext>
    {

        // Asset
        public AppPersistedGrantDbContext(
            DbContextOptions<AppPersistedGrantDbContext> options,
            OperationalStoreOptions store_options)
            : base(options, store_options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }
    }
}