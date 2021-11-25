
using APIServer.Aplication.Shared.Errors;
using HotChocolate.Types;
using SharedCore.Aplication.GraphQL.Types;

namespace APIServer.Aplication.GraphQL.Types
{

    public class BadRequestType : ObjectType<BadRequest>
    {
        protected override void Configure(IObjectTypeDescriptor<BadRequest> descriptor)
        {

            descriptor.Implements<BaseErrorInterfaceType>();
        }
    }

}