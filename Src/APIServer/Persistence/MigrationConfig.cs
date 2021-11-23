using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace APIServer.Persistence
{


    public class ApiDbContext_DesignContextFactory : IDesignTimeDbContextFactory<ApiDbContext>
    {

        public ApiDbContext CreateDbContext(string[] args)
        {

            var builder = new DbContextOptionsBuilder<ApiDbContext>();
            // builder.UseNpgsql("Host=localhost;Port=5555;Database=APIServer;Username=postgres;Password=postgres");
            builder.UseSqlite("Data Source=../Persistence/api.db");
            return new ApiDbContext(builder.Options);
        }
    }
}
