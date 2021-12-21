using HotChocolate.Types;
using APIServer.Aplication.Commands.WebHooks;

namespace APIServer.Aplication.GraphQL.Types
{
    public class UpdateWebHookPayloadPayloadType : ObjectType<UpdateWebHookPayload>
    {
        protected override void Configure(IObjectTypeDescriptor<UpdateWebHookPayload> descriptor)
        {
            descriptor.Field(e => e.hook).Type<WebHookType>();
        }
    }

    public class UpdateWebHookPayloadErrorUnion : UnionType<IUpdateWebHookError>
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