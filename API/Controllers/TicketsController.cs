using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketsController : ControllerBase
    {

        // GET api/tickets
        [HttpGet]
        public ActionResult<int> Get()
        {
            return 64;
        }
    }
}