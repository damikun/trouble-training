
using IdentityServer.Domain.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityServer.Persistence
{

    public class AppIdnetityDbContext : IdentityDbContext<ApplicationUser>
    {

        // Asset
        public AppIdnetityDbContext(DbContextOptions<AppIdnetityDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("citext");
            // modelBuilder.HasCollation("my_collation", locale: "en-u-ks-primary", provider: "icu", deterministic: false);

            // modelBuilder.Entity<Project>().OwnsOne(
            // p => p.Settings, od => {
            //     od.WithOwner(d => d.);
            //     od.Navigation(d => d.Order).UsePropertyAccessMode(PropertyAccessMode.Property);
            //     od.OwnsOne(c => c.BillingAddress);
            //     od.OwnsOne(c => c.ShippingAddress);
            // });

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppIdnetityDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
    }
}