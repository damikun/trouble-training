
using Device.Aplication.Shared.Errors;
using HotChocolate.Types;
using SharedCore.Aplication.GraphQL.Types;

namespace Device.Aplication.GraphQL.Types {

    public class InternalServerErrorType : ObjectType<InternalServerError> {
        protected override void Configure(IObjectTypeDescriptor<InternalServerError> descriptor) {
            descriptor.Implements<BaseErrorInterfaceType>();
        }
    }

}