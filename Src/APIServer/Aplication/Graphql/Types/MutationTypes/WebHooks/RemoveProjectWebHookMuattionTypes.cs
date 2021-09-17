using HotChocolate.Types;
using HotChocolate.Types.Relay;
using APIServer.Aplication.Commands.WebHooks;

namespace APIServer.Aplication.GraphQL.Types {
    public class RemoveWebHookPayloadType : ObjectType<RemoveWebHookPayload> {
        protected override void Configure(IObjectTypeDescriptor<RemoveWebHookPayload> descriptor) {
            descriptor.Field(e => e.removed_id).Type<IdType>().Resolve(ctx => {

                IIdSerializer serializer = ctx.Service<IIdSerializer>();

                return serializer.Serialize(default, "GQL_WebHook", ctx.Parent<RemoveWebHookPayload>().removed_id);
            });
        }
    }

    public class RemoveWebHookErrorUnion : UnionType<IRemoveWebHookError> {
        protected override void Configure(IUnionTypeDescriptor descriptor) {
            descriptor.Type<ValidationErrorType>();
            descriptor.Type<UnAuthorisedType>();
            descriptor.Type<InternalServerErrorType>();
            descriptor.Type<WebHookNotFoundType>();
        }
    }
}