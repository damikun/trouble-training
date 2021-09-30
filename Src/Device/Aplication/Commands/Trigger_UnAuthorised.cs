using MediatR;
using System.Threading;
using FluentValidation;
using System.Threading.Tasks;
using SharedCore.Aplication.Payload;

namespace Device.Aplication.Commands.Test {

    public class Trigger_UnAuthorised : IRequest<Trigger_UnAuthorisedPayload> {

    }

    /// <summary>
    /// Trigger_UnAuthorised Validator
    /// </summary>
    public class Trigger_UnAuthorisedValidator : AbstractValidator<Trigger_UnAuthorised> {

        public Trigger_UnAuthorisedValidator(){

        }
    }

    /// <summary>
    /// ITrigger_UnAuthorisedError
    /// </summary>
    public interface ITrigger_UnAuthorisedError { }

    /// <summary>
    /// Trigger_UnAuthorisedPayload
    /// </summary>
    public class Trigger_UnAuthorisedPayload : BasePayload<Trigger_UnAuthorisedPayload, ITrigger_UnAuthorisedError> {

    }

    /// <summary>Handler for <c>Trigger_UnAuthorised</c> command </summary>
    public class Trigger_UnAuthorisedHandler : IRequestHandler<Trigger_UnAuthorised, Trigger_UnAuthorisedPayload> {

        /// <summary>
        /// Main constructor
        /// </summary>
        public Trigger_UnAuthorisedHandler() {}

        /// <summary>
        /// Command handler for <c>Trigger_UnAuthorised</c>
        /// </summary>
        public async Task<Trigger_UnAuthorisedPayload> Handle(Trigger_UnAuthorised request, CancellationToken cancellationToken) {

                var response = Trigger_UnAuthorisedPayload.Success();

                await Task.CompletedTask;
                
                return response;
        }
    }
}