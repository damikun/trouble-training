using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using APIServer.Aplication.Commands.Internall;

namespace APIServer.Controllers
{

#if (!DEBUG)
    [ApiExplorerSettings(IgnoreApi = true)]
#endif

    public class TestController : BaseController
    {

        private readonly ILogger<HookController> _logger;


        public TestController(ILogger<HookController> logger)
        {
            _logger = logger;
        }


#if (!DEBUG)
        [NonAction]
#endif
        [Authorize]
        [HttpGet]
        public ActionResult TestClientCredentials()
        {
            return Ok("All good man :)");
        }

#if (!DEBUG)
        [NonAction]
#endif
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ClearDatabase()
        {
            if (Mediator != null)
            {
                await Mediator.Send(new ResetDatabase() { });
            }

            return Ok("Cleared");
        }

    }
}