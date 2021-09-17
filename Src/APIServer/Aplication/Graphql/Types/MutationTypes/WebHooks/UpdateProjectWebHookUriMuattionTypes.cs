using HotChocolate.Types;
using APIServer.Aplication.Commands.WebHooks;
using APIServer.Aplication.GraphQL.DTO;
using APIServer.Domain.Core.Models.WebHooks;

namespace APIServer.Aplication.GraphQL.Types {
    public class UpdateWebHookUriPayloadType : ObjectType<UpdateWebHookUriPayload> {
        protected override void Configure(IObjectTypeDescriptor<UpdateWebHookUriPayload> descriptor) {
            descriptor.Field(e => e.hook).Type<WebHookType>().Resolve(context => {
                WebHook e = context.Parent<UpdateWebHookUriPayload>().hook;

                if (e == null) {
                    return null;
                }

                return new GQL_WebHook {
                    ID = e.ID,
                    WebHookUrl = e.WebHookUrl,
                    ContentType = e.ContentType,
                    IsActive = e.IsActive,
                    LastTrigger = e.LastTrigger,
                    ListeningEvents = e.HookEvents
                };
            });
        }
    }

    public class UpdateWebHookUriErrorUnion : UnionType<IUpdateWebHookUriError> {
        protected override void Configure(IUnionTypeDescriptor descriptor) {
            descriptor.Type<ValidationErrorType>();
            descriptor.Type<UnAuthorisedType>();
            descriptor.Type<InternalServerErrorType>();
            descriptor.Type<WebHookNotFoundType>();
        }
    }
}