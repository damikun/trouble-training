using APIServer.Aplication.Commands.WebHooks;
using HotChocolate.Types;

namespace APIServer.Aplication.GraphQL.Types
{
    public class UpdateWebHookActivStatePayloadType : ObjectType<UpdateWebHookActivStatePayload>
    {
        protected override void Configure(IObjectTypeDescriptor<UpdateWebHookActivStatePayload> descriptor)
        {
            descriptor.Field(e => e.hook).Type<WebHookType>();
        }
    }

    public class UpdateWebHookActivStateErrorUnion : UnionType<IUpdateWebHookActivStateError>
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