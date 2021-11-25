
using APIServer.Aplication.Shared.Errors;
using HotChocolate.Types;
using SharedCore.Aplication.GraphQL.Types;

namespace APIServer.Aplication.GraphQL.Types
{

    public class WebHookNotFoundType : ObjectType<WebHookNotFound>
    {
        protected override void Configure(IObjectTypeDescriptor<WebHookNotFound> descriptor)
        {

            descriptor.Implements<BaseErrorInterfaceType>();
        }
    }

}