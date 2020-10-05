using Microsoft.AspNetCore.Mvc;
using Serilog.Events;

namespace MakingCodeGreatAgain.After.Logging
{
    [ApiController]
    [Route("[controller]")]
    public class LoggingLevelController : ControllerBase
    {
        [HttpPost]
        public IActionResult Post(LogEventLevel logEventLevel)
        {
            LoggingLevel.Update(logEventLevel);

            return Ok();
        }
    }
}