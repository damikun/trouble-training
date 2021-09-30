using MediatR;
using System.Threading;
using FluentValidation;
using System.Threading.Tasks;
using SharedCore.Aplication.Interfaces;
using Device.Aplication.Shared.Payload;

namespace Device.Aplication.Commands.Test {

    public class Hit_Endpoint_UnAuthorised : IRequest<Hit_Endpoint_UnAuthorisedPayload> {

    }

    /// <summary>
    /// Hit_Endpoint_UnAuthorised Validator
    /// </summary>
    public class Hit_Endpoint_UnAuthorisedValidator : AbstractValidator<Hit_Endpoint_UnAuthorised> {

        public Hit_Endpoint_UnAuthorisedValidator(){

        }
    }

    /// <summary>
    /// IHit_Endpoint_UnAuthorisedError
    /// </summary>
    public interface IHit_Endpoint_UnAuthorisedError { }

    /// <summary>
    /// Hit_Endpoint_UnAuthorisedPayload
    /// </summary>
    public class Hit_Endpoint_UnAuthorisedPayload : BasePayload<Hit_Endpoint_UnAuthorisedPayload, IHit_Endpoint_UnAuthorisedError> {

    }

    /// <summary>Handler for <c>Hit_Endpoint_UnAuthorised</c> command </summary>
    public class Hit_Endpoint_UnAuthorisedHandler : IRequestHandler<Hit_Endpoint_UnAuthorised, Hit_Endpoint_UnAuthorisedPayload> {

        /// <summary>
        /// Injected <c>ICurrentUser</c>
        /// </summary>
        private readonly ICurrentUser _current;

        /// <summary>
        /// Main constructor
        /// </summary>
        public Hit_Endpoint_UnAuthorisedHandler(
            ICurrentUser currentuser) {

            _current = currentuser;
        }

        /// <summary>
        /// Command handler for <c>Hit_Endpoint_UnAuthorised</c>
        /// </summary>
        public async Task<Hit_Endpoint_UnAuthorisedPayload> Handle(Hit_Endpoint_UnAuthorised request, CancellationToken cancellationToken) {

                var response = Hit_Endpoint_UnAuthorisedPayload.Success();

                await Task.CompletedTask;
                
                return response;
        }
    }
}