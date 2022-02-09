using System.Threading.Tasks;
using Device.Aplication.Commands.Test;
using MediatR;
using HotChocolate;
using HotChocolate.Types;

namespace Device.Aplication.GraphQL.Mutation
{

    /// <summary>
    /// Mutation
    /// </summary>
    [ExtendObjectType(OperationTypeNames.Mutation)]
    public class TriggerMutations
    {


        /// <summary>
        /// TriggerAuthorisedRequest
        /// </summary>
        /// <returns></returns>
        public async Task<Trigger_AuthorisedPayload> TriggerAuthorisedRequest(
            [Service] IMediator _mediator)
        {

            return await _mediator.Send(new Trigger_Authorised());
        }


        /// <summary>
        /// TriggerUnAuthorisedRequest
        /// </summary>
        /// <returns></returns>
        public async Task<Trigger_UnAuthorisedPayload> TriggerUnAuthorisedRequest(
            [Service] IMediator _mediator)
        {

            return await _mediator.Send(new Trigger_UnAuthorised());
        }
    }
}
