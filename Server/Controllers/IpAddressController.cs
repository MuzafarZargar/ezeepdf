using Microsoft.AspNetCore.Mvc;

namespace EzeePdf.Controllers
{
    [Route("api/get-ip")]
    [ApiController]
    public class IpAddressController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetClientIp()
        {
            var ip = HttpContext?.Connection?.RemoteIpAddress?.ToString();
            return Ok(ip);
        }
    }
}
