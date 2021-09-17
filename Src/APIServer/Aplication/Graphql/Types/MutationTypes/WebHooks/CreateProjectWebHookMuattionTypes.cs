using APIServer.Aplication.Commands.WebHooks;
using APIServer.Aplication.GraphQL.DTO;
using APIServer.Domain.Core.Models.WebHooks;
using HotChocolate.Types;

namespace APIServer.Aplication.GraphQL.Types {
    public class CreateWebHookPayloadType : ObjectType<CreateWebHookPayload> {
        protected override void Configure(IObjectTypeDescriptor<CreateWebHookPayload> descriptor) {
            descriptor.Field(e => e.hook).Type<WebHookType>().Resolver(context => {
                WebHook e = context.Parent<CreateWebHookPayload>().hook;

                if (e == null) {
                    return null;
                }

                return new GQL_WebHook {
                    ID = e.ID,
                    WebHookUrl = e.WebHookUrl,
                    // Secret = e.Secret,
                    ContentType = e.ContentType,
                    IsActive = e.IsActive,
                    LastTrigger = e.LastTrigger,
                    ListeningEvents = e.HookEvents
                };
            });
        }
    }

    public class CreateWebHookErrorUnion : UnionType<ICreateWebHookError> {
        protected override void Configure(IUnionTypeDescriptor descriptor) {
            descriptor.Type<ValidationErrorType>();
            descriptor.Type<UnAuthorisedType>();
            descriptor.Type<InternalServerErrorType>();
        }
    }
}