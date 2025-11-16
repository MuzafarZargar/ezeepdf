using Microsoft.AspNetCore.Mvc;

namespace EzeePdf.Controllers
{
    [Route("api/client-ip")]
    [ApiController]
    public class IpAddressController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetIp()
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            return Ok(new { ip });
        }
    }
}
