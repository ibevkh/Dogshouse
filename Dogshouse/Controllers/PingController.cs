using Microsoft.AspNetCore.Mvc;
using System.Reflection;

namespace Dogshouse.Controllers;


[ApiController]
[Route("[controller]")]
public class PingController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        var version = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "1.0.1";
        return Ok($"Dogshouseservice.Version{version}");
    }
}
