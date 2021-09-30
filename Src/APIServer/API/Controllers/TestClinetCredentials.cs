using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace APIServer.Controllers {

    [Authorize]
    public class TestAuthController : BaseController {

        private readonly ILogger<HookController> _logger;

        public TestAuthController(ILogger<HookController> logger) {
            _logger = logger;
        }

        /// <summary>
        /// This is hook test loopback echo controller
        /// </summary>
        [HttpGet]
        public ActionResult TestClientCredentials() {

            return Ok("All good man :)");

        }

    }
}
