using Microsoft.EntityFrameworkCore;
using APIServer.Domain.Core.Models.WebHooks;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace APIServer.Persistence
{
    public class WebHookonfiguration : IEntityTypeConfiguration<WebHook>
    {
        public void Configure(EntityTypeBuilder<WebHook> builder)
        {

            builder.HasKey(e => e.ID);

            builder.HasMany(e => e.Headers)
            .WithOne(e => e.WebHook)
            .HasForeignKey(e => e.WebHookID);

            builder.HasMany(e => e.Records)
            .WithOne(e => e.WebHook)
            .HasForeignKey(e => e.WebHookID);

            // builder.Property(e => e.HookEvents).HasConversion(
            //     new EnumArrToString_StringToEnumArr_Converter(
            //         e=> EnumArrToString_StringToEnumArr_Converter.Convert(e),
            //         s=> EnumArrToString_StringToEnumArr_Converter.Convert(s)
            //     )
            // );
        }
    }
}