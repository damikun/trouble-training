
using APIServer.Aplication.Interfaces;
using APIServer.Aplication.Shared.Errors;
using HotChocolate.Types;

namespace APIServer.Aplication.GraphQL.Types {

    public class BaseErrorType : ObjectType<BaseError> {
        protected override void Configure(IObjectTypeDescriptor<BaseError> descriptor) {

            descriptor.Interface<BaseErrorInterfaceType>();
        }
    }

    public class BaseErrorInterfaceType : InterfaceType<IBaseError> {
        protected override void Configure(IInterfaceTypeDescriptor<IBaseError> descriptor) {
            descriptor.Field(e => e.message).Type<StringType>();
        }
    }

}