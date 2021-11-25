
using APIServer.Aplication.Shared.Errors;
using HotChocolate.Types;

namespace APIServer.Aplication.GraphQL.Types
{

    public class ValidationErrorType : ObjectType<ValidationError>
    {
        protected override void Configure(IObjectTypeDescriptor<ValidationError> descriptor) { }
    }

}