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

        [HttpGet("{id}")]
        public ActionResult<string> GetOne(int id)
        {
            return "The query para is " + id;
        }
    }
}