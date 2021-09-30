using System.Threading.Tasks;
using Device.Aplication.Commands.Test;
using MediatR;
using HotChocolate;
using HotChocolate.Types;

namespace Device.Aplication.GraphQL.Mutation {

    /// <summary>
    /// Mutation
    /// </summary>
    [ExtendObjectType(OperationTypeNames.Mutation)]
    public class TriggerMutations {

        // /// <summary>
        // /// Triger test Authorised request to protected API
        // /// </summary>
        // public class TriggerAuthorisedRequestInput {

        // }

        /// <summary>
        /// Create new  webhook
        /// </summary>
        /// <returns></returns>
        public async Task<Trigger_AuthorisedPayload> TriggerAuthorisedRequest(
            // TriggerAuthorisedRequestInput request,
            [Service] IMediator _mediator) {

            return await _mediator.Send(new Trigger_Authorised());
        }

        // /// <summary>
        // /// Triger test UnAuthorised request to protected API
        // /// </summary>
        // public class TriggerUnAuthorisedRequestInput {

        // }

        /// <summary>
        /// Create new  webhook
        /// </summary>
        /// <returns></returns>
        public async Task<Trigger_UnAuthorisedPayload> TriggerUnAuthorisedRequest(
            // TriggerUnAuthorisedRequestInput request,
            [Service] IMediator _mediator) {

            return await _mediator.Send(new Trigger_UnAuthorised());
        }


    }

}
