
using APIServer.Aplication.Shared.Errors;
using HotChocolate.Types;

namespace APIServer.Aplication.GraphQL.Types {

    public class InternalServerErrorType : ObjectType<InternalServerError> {
        protected override void Configure(IObjectTypeDescriptor<InternalServerError> descriptor) {
            descriptor.Interface<BaseErrorInterfaceType>();
        }
    }

}