using Device.Aplication.Commands.Test;
using HotChocolate.Types;

namespace Device.Aplication.GraphQL.Types {
    public class TriggerAuthorisedPayloadType : ObjectType<Trigger_AuthorisedPayload> {
        protected override void Configure(IObjectTypeDescriptor<Trigger_AuthorisedPayload> descriptor) {
            
        }
    }

    public class TriggerAuthorisedErrorUnion : UnionType<ITrigger_AuthorisedError> {
        protected override void Configure(IUnionTypeDescriptor descriptor) {
            descriptor.Type<ValidationErrorType>();
            descriptor.Type<UnAuthorisedType>();
            descriptor.Type<InternalServerErrorType>();
        }
    }
}