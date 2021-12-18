using MediatR;
using System.Threading;
using FluentValidation;
using System.Threading.Tasks;
using SharedCore.Aplication.Payload;
using System.Net.Http;
using Device.Aplication.Shared.Errors;

namespace Device.Aplication.Commands.Test
{

    public class Trigger_Authorised : IRequest<Trigger_AuthorisedPayload>
    {

    }

    /// <summary>
    /// Trigger_Authorised Validator
    /// </summary>
    public class Trigger_AuthorisedValidator : AbstractValidator<Trigger_Authorised>
    {

        public Trigger_AuthorisedValidator()
        {

        }
    }

    /// <summary>
    /// ITrigger_AuthorisedError
    /// </summary>
    public interface ITrigger_AuthorisedError { }

    /// <summary>
    /// Trigger_AuthorisedPayload
    /// </summary>
    public class Trigger_AuthorisedPayload : BasePayload<Trigger_AuthorisedPayload, ITrigger_AuthorisedError>
    {


    }

    /// <summary>Handler for <c>Trigger_Authorised</c> command </summary>
    public class Trigger_AuthorisedHandler : IRequestHandler<Trigger_Authorised, Trigger_AuthorisedPayload>
    {

        /// <summary>
        /// Injected <c>IHttpClientFactory</c>
        /// </summary>
        private readonly IHttpClientFactory _clientFactory;

        /// <summary>
        /// Main constructor
        /// </summary>
        public Trigger_AuthorisedHandler(
            IHttpClientFactory clientFactory)
        {

            _clientFactory = clientFactory;
        }

        /// <summary>
        /// Command handler for <c>Trigger_Authorised</c>
        /// </summary>
        public async Task<Trigger_AuthorisedPayload> Handle(Trigger_Authorised request, CancellationToken cancellationToken)
        {
            var client = _clientFactory.CreateClient("test_auth_client");

            var client_response = await client.GetAsync("Test/TestClientCredentials", cancellationToken);

            if (client_response.IsSuccessStatusCode)
            {
                var response = Trigger_AuthorisedPayload.Success();
                return response;
            }
            else
            {
                var response = Trigger_AuthorisedPayload.Error(new InternalServerError(
                    string.Format("Failed to process api call status code: {0}", client_response.StatusCode)));

                return response;
            }
        }
    }
}