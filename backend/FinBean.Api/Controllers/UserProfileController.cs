using FinBean.Authentication.Services;
using FinBean.Core;
using FinBean.Database;
using FinBean.Database.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FinBean.Api.Controllers;

[Authorize]
[ApiController]
[Route("[controller]/[action]")]
public class UserProfileController : ControllerBase
{
    private readonly FinBeanDatabaseContext _context;
    private readonly CurrentUserService _currentUserService;

    public UserProfileController(FinBeanDatabaseContext context, CurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<UserProfile>), 200)]
    public async Task<IActionResult> GetUserProfiles()
    {
        var userProfiles = await _context.UserProfiles
            .Where(up => up.KeycloakUserId == _currentUserService.UserId)
            .ToListAsync();

        return Ok(userProfiles);
    }
}