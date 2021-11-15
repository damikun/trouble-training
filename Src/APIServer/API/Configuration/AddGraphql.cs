using System;
using HotChocolate;
using HotChocolate.Types;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using HotChocolate.Types.Pagination;
using Aplication.GraphQL.DataLoaders;
using APIServer.Aplication.GraphQL.Types;
using SharedCore.Aplication.GraphQL.Types;
using HotChocolate.Execution.Configuration;
using APIServer.Aplication.GraphQL.Queries;
using APIServer.Aplication.GraphQL.Mutation;
using APIServer.Aplication.GraphQL.Extensions;
using APIServer.Aplication.GraphQL.DataLoaders;
using Microsoft.Extensions.DependencyInjection;

namespace APIServer.Configuration
{
    public static partial class ServiceExtension
    {
        public static IServiceCollection AddGraphql(
            this IServiceCollection serviceCollection, IWebHostEnvironment env)
        {

            serviceCollection.AddGraphQLServer()
                    .SetPagingOptions(
                        new PagingOptions
                        {
                            IncludeTotalCount = true,
                            MaxPageSize = 200
                        })
                    .ModifyRequestOptions(requestExecutorOptions =>
                    {
                        if (env.IsDevelopment() ||
                            System.Diagnostics.Debugger.IsAttached)
                        {
                            requestExecutorOptions.ExecutionTimeout = TimeSpan.FromMinutes(1);
                        }

                        requestExecutorOptions.IncludeExceptionDetails = !env.IsProduction();
                    })
                    .AllowIntrospection(env.IsDevelopment())
                    .AddExportDirectiveType()

                    .ModifyOptions(options =>
                    {
                        options.UseXmlDocumentation = true;

                        options.SortFieldsByName = true;

                        options.RemoveUnreachableTypes = true;
                    })

                    .AddGlobalObjectIdentification()
                    .AddQueryFieldToMutationPayloads()

                    .AddHttpRequestInterceptor<IntrospectionInterceptor>()
                    .TryAddTypeInterceptor<StreamTypeInterceptor>()
                    .AddHttpRequestInterceptor<StreamRequestInterceptor>()

                    .AddFiltering()
                    .AddSorting()

                    .AddQueryType<QueryType>()
                        .AddTypeExtension<WebHookQueries>()
                        .AddTypeExtension<UserQueries>()
                        .AddTypeExtension<SystemQueries>()
                    .AddMutationType<Mutation>()
                        .AddTypeExtension<WebHookMutations>()

                    .BindRuntimeType<DateTime, DateTimeType>()
                    .BindRuntimeType<int, IntType>()
                    .BindRuntimeType<long, LongType>()

                    .AddType<BadRequestType>()
                    .AddType<InternalServerErrorType>()
                    .AddType<UnAuthorisedType>()
                    .AddType<ValidationErrorType>()
                    .AddType<BaseErrorType>()
                    .AddType<UserDeactivatedType>()
                    .AddType<BaseErrorInterfaceType>()
                    .AddType<WebHookNotFoundType>()
                    .AddType<WebHookRecordType>()
                    .AddType<WebHookType>()
                    .AddType<UserType>()
                    .AddType<UpdateWebHookUriPayloadType>()
                    .AddType<UpdateWebHookUriErrorUnion>()
                    .AddType<UpdateWebHookTriggerEventsPayloadType>()
                    .AddType<UpdateWebHookTriggerEventsErrorUnion>()
                    .AddType<UpdateWebHookSecretPayloadType>()
                    .AddType<UpdateWebHookSecretErrorUnion>()
                    .AddType<UpdateWebHookPayloadPayloadType>()
                    .AddType<UpdateWebHookPayloadErrorUnion>()
                    .AddType<UpdateWebHookActivStatePayloadType>()
                    .AddType<UpdateWebHookActivStateErrorUnion>()
                    .AddType<RemoveWebHookPayloadType>()
                    .AddType<RemoveWebHookErrorUnion>()
                    .AddType<CreateWebHookPayloadType>()
                    .AddType<CreateWebHookErrorUnion>()

                    .AddDataLoader<UserByIdDataLoader>()
                    .AddDataLoader<WebHookByIdDataLoader>()
                    .AddDataLoader<WebHookRecordByIdDataLoader>()

                    .UseCustomPipeline()
                    .UseReadPersistedQuery()
                    .AddReadOnlyFileSystemQueryStorage("./persisted_queries");

            return serviceCollection;
        }


        public static IRequestExecutorBuilder UseCustomPipeline(
            this IRequestExecutorBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            return builder
                .UseInstrumentations()
                .UseExceptions()
                .UseTimeout()
                .UseDocumentCache()
                .UseReadPersistedQuery()
                .UseDocumentParser()
                .UseDocumentValidation()
                .UseRequest<StreamArgumentRewriteMiddelware>() // Temporary workeround !
                .UseOperationCache()
                .UseOperationComplexityAnalyzer()
                .UseOperationResolver()
                .UseOperationVariableCoercion()
                .UseOperationExecution();
        }

    }
}