using FinBean.Database;
using FinBean.Database.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinBean.Api.Controllers;

[ApiController]
[Route("[controller]/[action]")]
public class UserProfileController : ControllerBase
{
    private readonly FinBeanDatabaseContext _context;

    public UserProfileController(FinBeanDatabaseContext context)
    {
        _context = context;
    }
    
    // TODO REMOVE LATER
    [HttpGet]
    [ProducesResponseType(typeof(UserProfile), 200)]
    public async Task<IActionResult> GetUserProfiles()
    {
        var userProfiles = await _context.UserProfiles.ToListAsync();
        return Ok(userProfiles);
    }
}