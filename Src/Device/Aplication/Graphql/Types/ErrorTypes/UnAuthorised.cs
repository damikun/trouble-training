using Device.Aplication.Shared.Errors;
using HotChocolate.Types;
using SharedCore.Aplication.GraphQL.Types;

namespace Device.Aplication.GraphQL.Types {

    public class UnAuthorisedType : ObjectType<UnAuthorised> {
        protected override void Configure(IObjectTypeDescriptor<UnAuthorised> descriptor) {
            descriptor.Implements<BaseErrorInterfaceType>();
        }
    }

}