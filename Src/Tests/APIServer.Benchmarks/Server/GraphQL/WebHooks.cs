using System;
using MediatR;
using System.Linq;
using HotChocolate;
using System.Threading;
using HotChocolate.Data;
using HotChocolate.Types;
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

        // Handled by downstream handler
        [UseConnection(typeof(WebHookType))]
        public async Task<Connection<GQL_WebHook>> WebhooksA(
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

        // Handled by HC middelware
        [UseApiDbContextAttribute]
        [UsePaging(typeof(WebHookType))]
        [UseFiltering]
        public IQueryable<GQL_WebHook> WebhooksB(
        [Service] ICurrentUser current,
        [ScopedService] ApiDbContext context)
        {

            if (!current.Exist)
            {
                return null;
            }

            return context.WebHooks
            .AsNoTracking()
            .Select(e => new GQL_WebHook
            {
                ID = e.ID,
                WebHookUrl = e.WebHookUrl,
                ContentType = e.ContentType,
                IsActive = e.IsActive,
                LastTrigger = e.LastTrigger,
                ListeningEvents = e.HookEvents
            })
            .AsQueryable();
        }
    }
}