using APIServer.Aplication.GraphQL.DataLoaders;
using APIServer.Aplication.GraphQL.Extensions;
using APIServer.Aplication.GraphQL.DTO;
using HotChocolate.Types.Pagination;
using APIServer.Aplication.Queries;
using HotChocolate.Resolvers;
using System.Threading.Tasks;
using HotChocolate.Types;
using System.Threading;
using MediatR;

namespace APIServer.Aplication.GraphQL.Types
{

    /// <summary>
    /// Graphql WebHookType
    /// </summary>
    public class WebHookType : ObjectType<GQL_WebHook>
    {

        protected override void Configure(IObjectTypeDescriptor<GQL_WebHook> descriptor)
        {
            descriptor.ImplementsNode().IdField(t => t.ID).ResolveNode((ctx, id) =>
                ctx.DataLoader<WebHookByIdDataLoader>().LoadAsync(id, ctx.RequestAborted));

            descriptor.Field(t => t.ID).Type<NonNullType<IdType>>();

            descriptor.Field("systemid").Type<NonNullType<LongType>>().Resolve((IResolverContext context) =>
            {
                return context.Parent<GQL_WebHook>().ID;
            });

            descriptor.Field(t => t.Records)
            .ResolveWith<WebHookResolvers>(e => e.WebHookHistory(default!, default!, default))
            .UseConnection<WebHookRecordType>();
        }

        //-----------------------------------------
        //-----------------------------------------

        private class WebHookResolvers
        {
            public async Task<Connection<GQL_WebHookRecord>> WebHookHistory(
               GQL_WebHookRecord hook,
               IResolverContext ctx,
               CancellationToken cancellationToken)
            {

                IMediator _mediator = ctx.Service<IMediator>();

                var command = new GetWebHooks()
                {
                    arguments = ctx.GetPaggingArguments()
                };

                var response = await _mediator.Send(new GetWebHookRecords()
                {
                    arguments = ctx.GetPaggingArguments(),
                    hook_id = ctx.Parent<GQL_WebHook>().ID
                });

                return response.connection;
            }
        }
    }
}
