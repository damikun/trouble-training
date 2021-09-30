using System;
using Microsoft.Extensions.DependencyInjection;
using HotChocolate;
using HotChocolate.Types;
using HotChocolate.Types.Pagination;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using SharedCore.Aplication.GraphQL.Types;
using Device.Aplication.GraphQL.Types;
using Device.Aplication.GraphQL.Queries;
using Device.Aplication.GraphQL.Mutation;

namespace Device.Configuration {
    public static partial class ServiceExtension {
        public static IServiceCollection AddGraphql(
            this IServiceCollection serviceCollection, IWebHostEnvironment env) {

            serviceCollection.AddGraphQLServer()
                    .SetPagingOptions(
                        new PagingOptions { IncludeTotalCount = true, MaxPageSize = 100 })
                    .ModifyRequestOptions(requestExecutorOptions => {
                        if (env.IsDevelopment() ||
                            System.Diagnostics.Debugger.IsAttached) {
                            requestExecutorOptions.ExecutionTimeout = TimeSpan.FromMinutes(1);
                        }
                        
                         requestExecutorOptions.IncludeExceptionDetails = !env.IsProduction();

                    })

                    // .AddGlobalObjectIdentification()
                    .AddQueryFieldToMutationPayloads()

                    .AddFiltering()
                    .AddSorting()

                    .AddQueryType<Query>()
                        .AddTypeExtension<SystemQueries>()
                    .AddMutationType<Mutation>()
                        .AddTypeExtension<TriggerMutations>()

                    .BindRuntimeType<DateTime, DateTimeType>()
                    .BindRuntimeType<int, IntType>()

                    .AddType<InternalServerErrorType>()
                    .AddType<UnAuthorisedType>()
                    .AddType<ValidationErrorType>()
                    .AddType<BaseErrorType>()
                    .AddType<BaseErrorInterfaceType>()

                    .AddType<TriggerUnAuthorisedPayloadType>()
                    .AddType<TriggerUnAuthorisedErrorUnion>()
                    .AddType<TriggerAuthorisedPayloadType>()
                    .AddType<TriggerAuthorisedErrorUnion>();

                
            return serviceCollection;
        }
    }
}