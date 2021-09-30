using Device.Aplication.Commands.Test;
using HotChocolate.Types;

namespace Device.Aplication.GraphQL.Types {
    public class TriggerUnAuthorisedPayloadType : ObjectType<Trigger_UnAuthorisedPayload> {
        protected override void Configure(IObjectTypeDescriptor<Trigger_UnAuthorisedPayload> descriptor) {

        }
    }

    public class TriggerUnAuthorisedErrorUnion : UnionType<ITrigger_UnAuthorisedError> {
        protected override void Configure(IUnionTypeDescriptor descriptor) {
            descriptor.Type<ValidationErrorType>();
            descriptor.Type<UnAuthorisedType>();
            descriptor.Type<InternalServerErrorType>();
        }
    }
}