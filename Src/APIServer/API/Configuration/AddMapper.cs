
using AutoMapper;
using APIServer.Aplication.Mapping;
using Microsoft.Extensions.DependencyInjection;

namespace APIServer.Configuration
{
    public static partial class ServiceExtension
    {
        public static IServiceCollection AddMapper(
      this IServiceCollection serviceCollection)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new WebHook_Mapping_Profile());
                mc.AddProfile(new WebHookRecords_Mapping_Profile());
            });

            IMapper mapper = mappingConfig.CreateMapper();

            serviceCollection.AddSingleton(mapper);

            return serviceCollection;
        }
    }
}