using HotChocolate.Types;
using HotChocolate.Resolvers;
using APIServer.Aplication.GraphQL.DTO;

namespace APIServer.Aplication.GraphQL.Types {

    /// <summary>
    /// Graphql UserType
    /// </summary>
    public class UserType : ObjectType<GQL_User> {

        protected override void Configure(IObjectTypeDescriptor<GQL_User> descriptor) {

            // descriptor.AsNode().IdField(t => t.ID).NodeResolver((ctx, id) =>
            //     ctx.DataLoader<WebHookByIdDataLoader>().LoadAsync(id, ctx.RequestAborted));

            descriptor.Field(t => t.Guid).Type<IdType>();

            descriptor.Field("systemid").Type<NonNullType<LongType>>().Resolve((IResolverContext context) => {
                return context.Parent<GQL_User>().Guid.ToString();
            });

        }

        private class UserTypeResolvers {


        }
    }
}
