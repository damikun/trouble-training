using APIServer.Aplication.Commands.WebHooks;
using APIServer.Aplication.GraphQL.DTO;
using APIServer.Domain.Core.Models.WebHooks;
using HotChocolate.Types;

namespace APIServer.Aplication.GraphQL.Types {
    public class UpdateWebHookSecretPayloadType : ObjectType<UpdateWebHookSecretPayload> {
        protected override void Configure(IObjectTypeDescriptor<UpdateWebHookSecretPayload> descriptor) {
            descriptor.Field(e => e.hook).Type<WebHookType>().Resolve(context => {
                WebHook e = context.Parent<UpdateWebHookSecretPayload>().hook;

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

    public class UpdateWebHookSecretErrorUnion : UnionType<IUpdateWebHookSecretError> {
        protected override void Configure(IUnionTypeDescriptor descriptor) {
            descriptor.Type<ValidationErrorType>();
            descriptor.Type<UnAuthorisedType>();
            descriptor.Type<InternalServerErrorType>();
            descriptor.Type<WebHookNotFoundType>();
        }
    }
}