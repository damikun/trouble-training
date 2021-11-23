using Duende.IdentityServer.EntityFramework.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Duende.IdentityServer.EntityFramework.DbContexts;

namespace IdentityServer.Persistence
{


    public class AppIdnetityDbContext_DesignContextFactory : IDesignTimeDbContextFactory<AppIdnetityDbContext>
    {

        public AppIdnetityDbContext CreateDbContext(string[] args)
        {

            var builder = new DbContextOptionsBuilder<AppIdnetityDbContext>();
            // builder.UseSqlite("Data Source=../Presistence/appDB.db");
            builder.UseNpgsql(IDesignTimeDbContextFactoryExtensions.GetDatabaseConnectionString());
            return new AppIdnetityDbContext(builder.Options);
        }
    }

    public class AppPersistedGrantDbContext_DesignContextFactory : IDesignTimeDbContextFactory<AppPersistedGrantDbContext>
    {
        public AppPersistedGrantDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<AppPersistedGrantDbContext>();
            var store = new OperationalStoreOptions();
            // builder.UseSqlite("Data Source=../Presistence/appDB.db");
            builder.UseNpgsql(IDesignTimeDbContextFactoryExtensions.GetDatabaseConnectionString());
            return new AppPersistedGrantDbContext(builder.Options, store);
        }
    }

    public class AppConfigurationDbContext_DesignContextFactory : IDesignTimeDbContextFactory<AppConfigurationDbContext>
    {
        public AppConfigurationDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<AppConfigurationDbContext>();
            var store = new ConfigurationStoreOptions();
            // builder.UseSqlite("Data Source=../Presistence/appDB.db");
            builder.UseNpgsql(IDesignTimeDbContextFactoryExtensions.GetDatabaseConnectionString());
            return new AppConfigurationDbContext(builder.Options, store);
        }
    }

    public static class IDesignTimeDbContextFactoryExtensions
    {
        public static string GetDatabaseConnectionString()
        {

            return "Host=localhost;Port=5555;Database=IdentityDB;Username=postgres;Password=postgres";
        }
    }

}
