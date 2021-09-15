
using APIServer.Aplication.Shared.Errors;
using HotChocolate.Types;

namespace APIServer.Aplication.GraphQL.Types {

    public class WebHookNotFoundType : ObjectType<WebHookNotFound> {
        protected override void Configure(IObjectTypeDescriptor<WebHookNotFound> descriptor) {

            descriptor.Interface<BaseErrorInterfaceType>();
        }
    }

}