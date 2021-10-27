using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace APIServer.Controllers {

    [AllowAnonymous]
    public class HookController : BaseController {

        private readonly ILogger<HookController> _logger;

        public HookController(ILogger<HookController> logger) {
            _logger = logger;
        }

        /// <summary>
        /// This is hook test loopback echo controller
        /// </summary>
        [HttpPost]
        public ActionResult HookLoopback([FromBody] object action) {

            dynamic parsedJson = JsonConvert.DeserializeObject(action.ToString());  

            return Ok();
        }

    }
}
