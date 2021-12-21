using APIServer.Aplication.Commands.WebHooks;
using HotChocolate.Types;

namespace APIServer.Aplication.GraphQL.Types
{
    public class UpdateWebHookSecretPayloadType : ObjectType<UpdateWebHookSecretPayload>
    {
        protected override void Configure(IObjectTypeDescriptor<UpdateWebHookSecretPayload> descriptor)
        {
            descriptor.Field(e => e.hook).Type<WebHookType>();
        }
    }

    public class UpdateWebHookSecretErrorUnion : UnionType<IUpdateWebHookSecretError>
    {
        protected override void Configure(IUnionTypeDescriptor descriptor)
        {
            descriptor.Type<ValidationErrorType>();
            descriptor.Type<UnAuthorisedType>();
            descriptor.Type<InternalServerErrorType>();
            descriptor.Type<WebHookNotFoundType>();
        }
    }
}