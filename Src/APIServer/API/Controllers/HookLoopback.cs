using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace APIServer.Controllers
{
    [AllowAnonymous]
    public class HookController : BaseController
    {

        private readonly ILogger<HookController> _logger;

        public HookController(ILogger<HookController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// This is hook test loopback echo controller
        /// </summary>
        [HttpPost]
        public ActionResult HookLoopback([FromBody] object action)
        {
            if (action.ToString() != null)
            {

                var str = action.ToString();

                if (str is not null && !string.IsNullOrWhiteSpace(str))
                {

                    dynamic parsedJson = JsonConvert.DeserializeObject(str)!;

                    return Ok();
                }
            }

            return BadRequest();
        }

    }
}
