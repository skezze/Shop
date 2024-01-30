using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Shop.Api.Controllers;
[ApiController, Route("api/[controller]")]
public class HomeController : ControllerBase
{
    public HomeController()
    {
    }
[HttpGet, Route("[action]")]
    public IActionResult GetHello()
    {
        return Ok(new {message = "hello"});
    }

}