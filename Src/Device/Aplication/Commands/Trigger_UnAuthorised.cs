using MediatR;
using System.Threading;
using FluentValidation;
using System.Threading.Tasks;
using SharedCore.Aplication.Payload;
using Device.Aplication.Shared.Errors;
using System.Net.Http;

namespace Device.Aplication.Commands.Test
{

    public class Trigger_UnAuthorised : IRequest<Trigger_UnAuthorisedPayload>
    {

    }

    /// <summary>
    /// Trigger_UnAuthorised Validator
    /// </summary>
    public class Trigger_UnAuthorisedValidator : AbstractValidator<Trigger_UnAuthorised>
    {

        public Trigger_UnAuthorisedValidator()
        {

        }
    }

    /// <summary>
    /// ITrigger_UnAuthorisedError
    /// </summary>
    public interface ITrigger_UnAuthorisedError { }

    /// <summary>
    /// Trigger_UnAuthorisedPayload
    /// </summary>
    public class Trigger_UnAuthorisedPayload : BasePayload<Trigger_UnAuthorisedPayload, ITrigger_UnAuthorisedError>
    {

    }

    /// <summary>Handler for <c>Trigger_UnAuthorised</c> command </summary>
    public class Trigger_UnAuthorisedHandler : IRequestHandler<Trigger_UnAuthorised, Trigger_UnAuthorisedPayload>
    {

        /// <summary>
        /// Injected <c>IHttpClientFactory</c>
        /// </summary>
        private readonly IHttpClientFactory _clientFactory;

        /// <summary>
        /// Main constructor
        /// </summary>
        public Trigger_UnAuthorisedHandler(
            IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        /// <summary>
        /// Command handler for <c>Trigger_UnAuthorised</c>
        /// </summary>
        public async Task<Trigger_UnAuthorisedPayload> Handle(Trigger_UnAuthorised request, CancellationToken cancellationToken)
        {


            var client = _clientFactory.CreateClient("client_without_token_managment");

            var client_response = await client.GetAsync("Test/TestClientCredentials", cancellationToken);

            if (client_response.IsSuccessStatusCode)
            {
                var response = Trigger_UnAuthorisedPayload.Success();
                return response;
            }
            else
            {
                var response = Trigger_UnAuthorisedPayload.Error(new InternalServerError(
                    string.Format("Failed to process api call status code: {0}", client_response.StatusCode)));

                return response;
            }
        }
    }
}