using MediatR;
using System.Threading;
using FluentValidation;
using System.Threading.Tasks;
using Device.Aplication.Shared.Payload;
using System.Net.Http;

namespace Device.Aplication.Commands.Test {

    public class Hit_Endpoint_Authorised : IRequest<Hit_Endpoint_AuthorisedPayload> {

    }

    /// <summary>
    /// Hit_Endpoint_Authorised Validator
    /// </summary>
    public class Hit_Endpoint_AuthorisedValidator : AbstractValidator<Hit_Endpoint_Authorised> {

        public Hit_Endpoint_AuthorisedValidator(){

        }
    }

    /// <summary>
    /// IHit_Endpoint_AuthorisedError
    /// </summary>
    public interface IHit_Endpoint_AuthorisedError { }

    /// <summary>
    /// Hit_Endpoint_AuthorisedPayload
    /// </summary>
    public class Hit_Endpoint_AuthorisedPayload : BasePayload<Hit_Endpoint_AuthorisedPayload, IHit_Endpoint_AuthorisedError> {

    }

    /// <summary>Handler for <c>Hit_Endpoint_Authorised</c> command </summary>
    public class Hit_Endpoint_AuthorisedHandler : IRequestHandler<Hit_Endpoint_Authorised, Hit_Endpoint_AuthorisedPayload> {

        /// <summary>
        /// Injected <c>IHttpClientFactory</c>
        /// </summary>
        private readonly IHttpClientFactory _clientFactory;

        /// <summary>
        /// Main constructor
        /// </summary>
        public Hit_Endpoint_AuthorisedHandler(
            IHttpClientFactory clientFactory) {

            _clientFactory = clientFactory;
        }

        /// <summary>
        /// Command handler for <c>Hit_Endpoint_Authorised</c>
        /// </summary>
        public async Task<Hit_Endpoint_AuthorisedPayload> Handle(Hit_Endpoint_Authorised request, CancellationToken cancellationToken) {

            var client = _clientFactory.CreateClient("test_auth_client");

            var client_response = await client.GetAsync("TestAuth/TestClientCredentials", cancellationToken);

            if(client_response.IsSuccessStatusCode ){
                var response = Hit_Endpoint_AuthorisedPayload.Success();
                
                return response;
            }else{
                var response = Hit_Endpoint_AuthorisedPayload.Error();
                
                return response;
            }
        }
    }
}