using Duende.IdentityServer.Extensions;

using IdentityService.Models;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IdentityService.Controllers;

public class AccountController : Controller
{
    private readonly IConfiguration _configuration;

    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController(
        IConfiguration configuration,
        SignInManager<ApplicationUser> signInManager)
    {
        _configuration = configuration;
        _signInManager = signInManager;
    }

    [HttpGet]
    public async Task<IActionResult> Signout()
    {
        if (!User.IsAuthenticated())
            return Ok();

        try
        {
            await _signInManager.SignOutAsync();
        }
        catch
        {
            return BadRequest();
        }

        return Ok();
    }
}
