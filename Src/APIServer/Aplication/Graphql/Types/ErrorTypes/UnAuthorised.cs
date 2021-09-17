using APIServer.Aplication.Shared.Errors;
using HotChocolate.Types;

namespace APIServer.Aplication.GraphQL.Types {

    public class UnAuthorisedType : ObjectType<UnAuthorised> {
        protected override void Configure(IObjectTypeDescriptor<UnAuthorised> descriptor) {
            descriptor.Implements<BaseErrorInterfaceType>();
        }
    }

}