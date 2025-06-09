using Microsoft.AspNetCore.Mvc;

namespace ZendeskApiCore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AddressController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetAddress()
        {
            return Ok("Address");
        }
    }
}
