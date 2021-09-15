using HotChocolate.Types;
using APIServer.Aplication.Commands.WebHooks;
using APIServer.Domain.Core.Models.WebHooks;
using APIServer.Aplication.GraphQL.DTO;

namespace APIServer.Aplication.GraphQL.Types {
    public class UpdateWebHookPayloadPayloadType : ObjectType<UpdateWebHookPayload> {
        protected override void Configure(IObjectTypeDescriptor<UpdateWebHookPayload> descriptor) {
            descriptor.Field(e => e.hook).Type<WebHookType>().Resolver(context => {
                WebHook e = context.Parent<UpdateWebHookPayload>().hook;

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

    public class UpdateWebHookPayloadErrorUnion : UnionType<IUpdateWebHookError> {
        protected override void Configure(IUnionTypeDescriptor descriptor) {
            descriptor.Type<ValidationErrorType>();
            descriptor.Type<UnAuthorisedType>();
            descriptor.Type<InternalServerErrorType>();
            descriptor.Type<WebHookNotFoundType>();
        }
    }
}