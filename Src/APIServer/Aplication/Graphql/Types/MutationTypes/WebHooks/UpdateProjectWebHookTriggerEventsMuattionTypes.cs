using APIServer.Aplication.Commands.WebHooks;
using HotChocolate.Types;

namespace APIServer.Aplication.GraphQL.Types
{
    public class UpdateWebHookTriggerEventsPayloadType : ObjectType<UpdateWebHookTriggerEventsPayload>
    {
        protected override void Configure(IObjectTypeDescriptor<UpdateWebHookTriggerEventsPayload> descriptor)
        {
            descriptor.Field(e => e.hook).Type<WebHookType>();
        }
    }

    public class UpdateWebHookTriggerEventsErrorUnion : UnionType<IUpdateWebHookTriggerEventsError>
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