using HotChocolate.Types;
using APIServer.Aplication.Commands.WebHooks;

namespace APIServer.Aplication.GraphQL.Types
{
    public class UpdateWebHookUriPayloadType : ObjectType<UpdateWebHookUriPayload>
    {
        protected override void Configure(IObjectTypeDescriptor<UpdateWebHookUriPayload> descriptor)
        {
            descriptor.Field(e => e.hook).Type<WebHookType>();
        }
    }

    public class UpdateWebHookUriErrorUnion : UnionType<IUpdateWebHookUriError>
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