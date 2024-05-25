using IdentityService.Models;

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace IdentityService.Controllers;

public class AccountController : Controller
{
    private readonly SignInManager<ApplicationUser> _signInManager;

    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccountController(
        SignInManager<ApplicationUser> signInManager,
        IHttpContextAccessor httpContextAccessor)
    {
        _signInManager = signInManager;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpGet]
    public async Task<IActionResult> Signout()
    {
        try
        {
            await _signInManager.SignOutAsync();

            await _httpContextAccessor.HttpContext?.SignOutAsync();

            Response.Headers["Set-Cookie"] = ".AspNetCore.Identity.Application=; Expires=; Path=/;";
        }
        catch
        {
            return BadRequest();
        }

        return Ok();
    }
}
