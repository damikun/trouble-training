using HotChocolate;
using HotChocolate.Types;
using System.Threading.Tasks;
using System.Threading;
using HotChocolate.Resolvers;
using HotChocolate.Types.Relay;
using APIServer.Aplication.GraphQL.DTO;
using APIServer.Aplication.GraphQL.DataLoaders;
using SharedCore.Aplication.Interfaces;

namespace APIServer.Aplication.GraphQL.Types {
#pragma warning disable 612, 618 
    /// <summary>
    /// Graphql WebHookType
    /// </summary>
    public class WebHookRecordType : ObjectType<GQL_WebHookRecord> {

        protected override void Configure(IObjectTypeDescriptor<GQL_WebHookRecord> descriptor) {

            descriptor.AsNode().IdField(t => t.ID).NodeResolver((ctx, id) =>
                ctx.DataLoader<WebHookRecordByIdDataLoader>().LoadAsync(id, ctx.RequestAborted));

            descriptor.Field(t => t.ID).Type<NonNullType<IdType>>();

            descriptor.Field("systemid").Type<NonNullType<LongType>>().Resolve((IResolverContext context) => {
                return context.Parent<GQL_WebHookRecord>().ID;
            });

            descriptor.Field(t => t.WebHookID).Type<IdType>().Resolver(ctx => {

                IIdSerializer serializer = ctx.Service<IIdSerializer>();

                return serializer.Serialize(default, "GQL_WebHook", ctx.Parent<GQL_WebHookRecord>().WebHookID);
            });

            descriptor.Field(e => e.WebHookSystemID).Type<NonNullType<LongType>>().Resolve((IResolverContext context) => {
                return context.Parent<GQL_WebHookRecord>().WebHookID;
            });

            descriptor.Field(t => t.WebHook)
            .ResolveWith<WebHookRecordResolvers>(e => e.GetWebHook(default!, default!, default!, default, default));
        }

        private class WebHookRecordResolvers {
            public async Task<GQL_WebHook> GetWebHook(
            GQL_WebHookRecord hook,
            WebHookByIdDataLoader loader,
            IResolverContext context,
            CancellationToken cancellationToken,
            [Service] ICurrentUser current) {

                if (hook.ID <= 0 || !current.Exist) {
                    return null;
                }

                return await loader.LoadAsync(hook.ID, cancellationToken);
            }
        }
    }

#pragma warning restore 612, 618

}
