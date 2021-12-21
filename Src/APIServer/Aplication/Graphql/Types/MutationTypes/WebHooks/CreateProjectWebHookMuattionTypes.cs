using APIServer.Aplication.Commands.WebHooks;
using HotChocolate.Types;

namespace APIServer.Aplication.GraphQL.Types
{
    public class CreateWebHookPayloadType : ObjectType<CreateWebHookPayload>
    {
        protected override void Configure(IObjectTypeDescriptor<CreateWebHookPayload> descriptor)
        {
            descriptor.Field(e => e.hook)
            .Type<WebHookType>();
        }
    }

    public class CreateWebHookErrorUnion : UnionType<ICreateWebHookError>
    {
        protected override void Configure(IUnionTypeDescriptor descriptor)
        {
            descriptor.Type<ValidationErrorType>();
            descriptor.Type<UnAuthorisedType>();
            descriptor.Type<InternalServerErrorType>();
        }
    }
}