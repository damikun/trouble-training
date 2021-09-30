
using Device.Aplication.Shared.Errors;
using HotChocolate.Types;

namespace Device.Aplication.GraphQL.Types {

    public class ValidationErrorType : ObjectType<ValidationError> {
        protected override void Configure(IObjectTypeDescriptor<ValidationError> descriptor) { }
    }

}