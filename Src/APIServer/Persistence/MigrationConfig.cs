using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace APIServer.Persistence {


    public class ApiDbContext_DesignContextFactory : IDesignTimeDbContextFactory<ApiDbContext> {
        
        public ApiDbContext CreateDbContext(string[] args) {

           var builder = new DbContextOptionsBuilder<ApiDbContext>();
            builder.UseNpgsql("Host=localhost;Port=5432;Database=APIServer;Username=postgres;Password=postgres");
            return new ApiDbContext(builder.Options);
        }
    }
}
