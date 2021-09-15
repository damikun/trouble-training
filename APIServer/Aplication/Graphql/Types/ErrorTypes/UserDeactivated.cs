
using APIServer.Aplication.Shared.Errors;
using HotChocolate.Types;

namespace APIServer.Aplication.GraphQL.Types {

    public class UserDeactivatedType : ObjectType<UserDeactivated> {
        protected override void Configure(IObjectTypeDescriptor<UserDeactivated> descriptor) {

            descriptor.Interface<BaseErrorInterfaceType>();
        }
    }

}