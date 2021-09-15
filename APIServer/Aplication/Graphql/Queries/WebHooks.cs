using HotChocolate;
using HotChocolate.Types;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using HotChocolate.Types.Relay;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Collections.Generic;
using MediatR;
using System;
using APIServer.Aplication.GraphQL.Types;
using APIServer.Aplication.GraphQL.DTO;
using Shared.Aplication.Interfaces;
using APIServer.Aplication.GraphQL.DataLoaders;
using APIServer.Persistence;
using APIServer.Domain.Core.Models.WebHooks;
using APIServer.Aplication.GraphQL.Extensions;

namespace APIServer.Aplication.GraphQL.Queries {

    /// <summary>
    ///  Webhook Querys
    /// </summary>
    [ExtendObjectType(Name = "Querry")]
    public class WebHookQueries {

        /// <summary>
        /// Return collection of webhook records
        /// </summary>
        [UseApiDbContextAttribute]
        [UsePaging(typeof(WebHookRecordType))]
        [UseFiltering]
        public async Task<IQueryable<GQL_WebHook>> Webhooks(
        [Service] ICurrentUser current,
        [ScopedService] ApiDbContext context) {

            if (!current.Exist) {
                return null;
            }
            
            return context.WebHooks
                .AsNoTracking()
                .Select(e=>  new GQL_WebHook {
                    ID = e.ID,
                    WebHookUrl = e.WebHookUrl,
                    // Secret = e.Secret,
                    ContentType = e.ContentType,
                    IsActive = e.IsActive,
                    LastTrigger = e.LastTrigger,
                    ListeningEvents = e.HookEvents
                });
        }

        /// <summary>
        /// Return collection of webhook records
        /// </summary>
        [UsePaging(typeof(WebHookRecordType))]
        [UseFiltering]
        public async Task<IEnumerable<GQL_WebHookRecord>> GetWebHookRecords(
        [ID(nameof(GQL_WebHook))] long hook_id,
        [Service] IMediator mediator,
        [Service] ICurrentUser current,
        [Service] IDbContextFactory<ApiDbContext> factory) {

            if (!current.Exist || hook_id <= 0) {
                return null;
            }

            await using ApiDbContext dbContext = 
                factory.CreateDbContext();

            return dbContext.WebHooksHistory
            .AsNoTracking()
            .Where(e => e.WebHookID == hook_id)
            .Select(e => new GQL_WebHookRecord() {
                ID = e.ID,
                WebHookID = e.WebHookID,
                WebHookSystemID = e.WebHookID,
                StatusCode = e.StatusCode,
                ResponseBody = e.ResponseBody,
                RequestBody = e.RequestBody,
                RequestHeaders = e.RequestHeaders,
                Guid = e.Guid,
                Result = e.Result,
                TriggerType = e.HookType,
                Exception = e.Exception,
                Timestamp = e.Timestamp,
            }!).OrderByDescending(e => e.Timestamp);
        }

        /// <summary>
        /// Returns Webook supported event triggers
        /// </summary>
        public async Task<IEnumerable<string>> GetWebHookEventsTriggers() {

            return Enum.GetNames(typeof(HookEventType)).ToList();
        }

        /// <summary>
        /// Get WebHook Record by ID
        /// </summary>
        public async Task<GQL_WebHookRecord> GetWebHookRecord(
        [ID(nameof(GQL_WebHookRecord))] long hook_record_id,
        [Service] WebHookRecordByIdDataLoader loader,
        [Service] IHttpContextAccessor httpcontext,
        [Service] ICurrentUser current) {

            if (!current.Exist) {
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
        [Service] ICurrentUser current) {

            if (!current.Exist || webhook_id <= 0) {
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
        [Service] ICurrentUser current) {

            if (!current.Exist || webhook_id <= 0) {
                return null;
            }

            return await loader.LoadAsync(webhook_id, httpcontext.HttpContext.RequestAborted);
        }

    }
}
