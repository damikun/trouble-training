using System;
using MediatR;
using System.Linq;
using HotChocolate;
using System.Threading;
using HotChocolate.Types;
using APIServer.Persistence;
using System.Threading.Tasks;
using HotChocolate.Resolvers;
using HotChocolate.Types.Relay;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using APIServer.Aplication.Queries;
using HotChocolate.Types.Pagination;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using SharedCore.Aplication.Interfaces;
using APIServer.Aplication.GraphQL.DTO;
using APIServer.Aplication.GraphQL.Types;
using APIServer.Domain.Core.Models.WebHooks;
using APIServer.Aplication.GraphQL.Extensions;
using APIServer.Aplication.GraphQL.DataLoaders;

namespace APIServer.Aplication.GraphQL.Queries
{
    /// <summary>
    ///  Webhook Queries
    /// </summary>
    [ExtendObjectType(OperationTypeNames.Query)]
    public class WebHookQueries
    {
        /// <summary>
        /// Return collection of webhook records
        /// </summary>
        [UseConnection(typeof(WebHookType))]
        public async Task<Connection<GQL_WebHook>> Webhooks(
        IResolverContext ctx,
        [Service] IMediator mediator)
        {
            var command = new GetWebHooks()
            {
                arguments = ctx.GetPaggingArguments()
            };

            var response = await mediator.Send(command);

            return response.connection;
        }

        /// <summary>
        /// Return collection of webhook records
        /// </summary>
        [UseConnection(typeof(WebHookRecordType))]
        public async Task<Connection<GQL_WebHookRecord>> GetWebHookRecords(
        [ID(nameof(GQL_WebHook))] long hook_id,
        IResolverContext ctx,
        [Service] IMediator mediator)
        {
            var command = new GetWebHookRecords()
            {
                hook_id = hook_id,
                arguments = ctx.GetPaggingArguments()
            };

            var response = await mediator.Send(command);

            return response.connection;
        }

        /// <summary>
        /// Returns Webook supported event triggers
        /// </summary>
        public IEnumerable<string> GetWebHookEventsTriggers()
        {
            return Enum.GetNames(typeof(HookEventType)).ToList();
        }

        /// <summary>
        /// Get WebHook Record by ID
        /// </summary>
        public async Task<GQL_WebHookRecord> GetWebHookRecord(
        [ID(nameof(GQL_WebHookRecord))] long hook_record_id,
        [Service] WebHookRecordByIdDataLoader loader,
        [Service] IHttpContextAccessor httpcontext,
        [Service] ICurrentUser current)
        {

            if (!current.Exist)
            {
                return null;
            }

            return await loader.LoadAsync(hook_record_id, httpcontext.HttpContext.RequestAborted);
        }

        /// <summary>
        /// Return webhook by gql ID
        /// </summary>
        public async Task<GQL_WebHook> GetWebHookById(
        [ID(nameof(GQL_WebHook))] long webhook_id,
        [Service] WebHookByIdDataLoader loader,
        [Service] IHttpContextAccessor httpcontext,
        [Service] ICurrentUser current)
        {

            if (!current.Exist || webhook_id <= 0)
            {
                return null;
            }

            return await loader.LoadAsync(webhook_id, httpcontext.HttpContext.RequestAborted);
        }

        /// <summary>
        /// Return webhook by system (DB) Id
        /// </summary>
        public async Task<GQL_WebHook> GetWebHookBySystemId(
        long webhook_id,
        [Service] WebHookByIdDataLoader loader,
        [Service] IHttpContextAccessor httpcontext,
        [Service] ICurrentUser current)
        {

            if (!current.Exist || webhook_id <= 0)
            {
                return null;
            }

            return await loader.LoadAsync(webhook_id, httpcontext.HttpContext.RequestAborted);
        }

        public async IAsyncEnumerable<GQL_WebHook> WebHooksTestStream(
        [Service] ICurrentUser current,
        [Service] IDbContextFactory<ApiDbContext> factory,
        [EnumeratorCancellation] CancellationToken ct)
        {

            if (current.Exist)
            {
                var db_context = factory.CreateDbContext();

                var stream = db_context.WebHooks
                    .AsNoTracking()
                    .Select(e => new GQL_WebHook
                    {
                        ID = e.ID,
                        WebHookUrl = e.WebHookUrl,
                        ContentType = e.ContentType,
                        IsActive = e.IsActive,
                        LastTrigger = e.LastTrigger,
                        ListeningEvents = e.HookEvents
                    }).AsAsyncEnumerable();

                await foreach (var hook in stream.WithCancellation(ct))
                {
                    yield return hook;

                    Thread.Sleep(350); // Just to slow since DB is fast
                }
            }
        }
    }
}