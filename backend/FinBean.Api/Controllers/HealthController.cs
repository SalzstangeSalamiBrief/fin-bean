using FinBean.Database.Models;
using Microsoft.AspNetCore.Mvc;

namespace FinBean.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class HealthController: ControllerBase
{
    [HttpGet]
    [ProducesResponseType(200)]
    public IActionResult IsHealth()
    {
        return Ok();
    }
}