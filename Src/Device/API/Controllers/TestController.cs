using System.Threading.Tasks;
using Device.Aplication.Commands.Test;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Device.Controllers {

    [AllowAnonymous]
    public class TestController : BaseController {

        private readonly ILogger<TestController> _logger;

        private readonly IMediator _mediator;

        public TestController(
            ILogger<TestController> logger,
            IMediator mediator) {
            _logger = logger;
            _mediator = mediator;
        }

        /// <summary>
        /// This is hook test loopback echo controller
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Test() {

            await _mediator.Send(new Trigger_Authorised());
         
            return Ok();

        }

    }
}
