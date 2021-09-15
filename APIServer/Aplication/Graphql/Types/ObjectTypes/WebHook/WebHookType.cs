using System.Linq;
using HotChocolate.Types;
using System.Collections.Generic;
using HotChocolate.Resolvers;
using Microsoft.EntityFrameworkCore;
using APIServer.Aplication.GraphQL.DTO;
using APIServer.Aplication.GraphQL.DataLoaders;
using Shared.Aplication.Interfaces;
using APIServer.Persistence;

namespace APIServer.Aplication.GraphQL.Types {
#pragma warning disable 612, 618 
    /// <summary>
    /// Graphql WebHookType
    /// </summary>
    public class WebHookType : ObjectType<GQL_WebHook> {

        protected override void Configure(IObjectTypeDescriptor<GQL_WebHook> descriptor) {

            descriptor.AsNode().IdField(t => t.ID).NodeResolver((ctx, id) =>
                ctx.DataLoader<WebHookByIdDataLoader>().LoadAsync(id, ctx.RequestAborted));

            descriptor.Field(t => t.ID).Type<NonNullType<IdType>>();

            descriptor.Field("systemid").Type<NonNullType<LongType>>().Resolve((IResolverContext context) => {
                return context.Parent<GQL_WebHook>().ID;
            });

            descriptor.Field(t => t.Records)
            .Resolve(async  ctx => {

                IDbContextFactory<ApiDbContext> _factory = ctx.Service<IDbContextFactory<ApiDbContext>>();

                ICurrentUser _current = ctx.Service<ICurrentUser>();

                long hook_id = ctx.Parent<GQL_WebHook>().ID;

                if (!_current.Exist) {
                    return null;
                }

                if (hook_id <= 0) {
                    return new List<GQL_WebHookRecord>().AsQueryable();
                }

                await using ApiDbContext dbContext = 
                    _factory.CreateDbContext();
            

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
                    TriggerType = e.HookType,
                    Result = e.Result,
                    Guid = e.Guid,
                    RequestHeaders = e.RequestHeaders,
                    Exception = e.Exception,
                    Timestamp = e.Timestamp,
                }!).OrderByDescending(e => e.Timestamp);

            })
            .UsePaging<WebHookRecordType>()
            .UseFiltering();

        }

        private class ProjectResolvers {


        }
    }
#pragma warning restore 612, 618

}
