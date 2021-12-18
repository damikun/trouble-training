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
using HotChocolate.AspNetCore.Extensions;
using HotChocolate.AspNetCore;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using Microsoft.Extensions.Primitives;
using GraphQL.Server.Ui.Playground;
using GraphQL.Server.Ui.Voyager;

namespace APIServer.Configuration
{
    public static partial class ServiceExtension
    {
        private const string Endpoint_path = "/graphql";
        private const string BCP_path = "/bcp";
        private const string Playground_path = "/playground";
        private const string Voyager_path = "/voyager";
        private const string GA_Tracking = null;
        private const string Persisted_Queries_path = "./persisted_queries";

        //--------------------------------------------------

        public static IServiceCollection AddGraphql(
            this IServiceCollection serviceCollection,
            IWebHostEnvironment env)
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
                .AddReadOnlyFileSystemQueryStorage(Persisted_Queries_path);

            return serviceCollection;
        }

        //--------------------------------------------------

        public static IRequestExecutorBuilder UseCustomPipeline(
            this IRequestExecutorBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            // !The order of call defines pipeline!

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

        //--------------------------------------------------

        public static GraphQLEndpointConventionBuilder MapGraphQLEndpoint(
            this IEndpointRouteBuilder builder)
        {
            var env = builder.ServiceProvider.GetService<IWebHostEnvironment>();

            return builder.MapGraphQL()
            .WithOptions(new GraphQLServerOptions
            {

                EnableSchemaRequests = env.IsDevelopment(),
                Tool = {
                    Enable = env.IsDevelopment(),
                }
            });
        }

        //--------------------------------------------------

        public static BananaCakePopEndpointConventionBuilder MapBCPEndpoint(
            this IEndpointRouteBuilder builder)
        {
            var env = builder.ServiceProvider.GetService<IWebHostEnvironment>();

            return builder.MapBananaCakePop(BCP_path)
                .WithOptions(new GraphQLToolOptions
                {
                    Enable = env.IsDevelopment(),
                    DisableTelemetry = true,
                    HttpHeaders = new HeaderDictionary(
                            new Dictionary<string, StringValues>()
                            {
                                { "x-csrf","1" }
                            }
                        ),
                    GaTrackingId = GA_Tracking ?? "G-VZY9HR8VLJ",
                    UseBrowserUrlAsGraphQLEndpoint = true,
                    GraphQLEndpoint = Endpoint_path,
                });
        }

        //--------------------------------------------------

        public static IApplicationBuilder UsePlayground(
            this IApplicationBuilder builder)
        {
            var env = builder.ApplicationServices.GetService<IWebHostEnvironment>();

            var opt = new PlaygroundOptions()
            {
                SchemaPollingEnabled = true,
                SchemaPollingInterval = 5000,
                // GraphQLEndPoint = accesor.HttpContext?.Request?.GetDisplayUrl(),
                RequestCredentials = RequestCredentials.Include,
                Headers = new Dictionary<string, object>()
                {
                    { "x-csrf",1}
                }
            };

            return builder.UseGraphQLPlayground(opt, Playground_path);

        }

        //--------------------------------------------------

        public static IApplicationBuilder UseVoyager(
            this IApplicationBuilder builder)
        {
            var env = builder.ApplicationServices.GetService<IWebHostEnvironment>();

            var opt = new VoyagerOptions()
            {
                Headers = new Dictionary<string, object>()
                {
                    { "x-csrf",1}
                }
            };

            return builder.UseGraphQLVoyager(opt, Voyager_path);

        }


    }
}