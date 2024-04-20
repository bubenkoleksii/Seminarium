using System.Security.Claims;

using IdentityModel;

using IdentityService.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityService.Pages.Account.Register;

[SecurityHeaders]
[AllowAnonymous]
public class IndexModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;

    private readonly IConfiguration _configuration;

    public IndexModel(UserManager<ApplicationUser> userManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _configuration = configuration;
    }

    [BindProperty]
    public RegisterViewModel Input { get; set; }

    [BindProperty]
    public bool RegisterSuccess { get; set; }

    public IActionResult OnGet()
    {
        Input = new RegisterViewModel
        {
            ClientHomeUrl = _configuration["ClientHomeUrl"]!
        };

        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        var clientHomeUrl = _configuration["ClientHomeUrl"]!;
        Input.ClientHomeUrl = clientHomeUrl;

        if (Input.Button != "register") return Redirect(clientHomeUrl);

        if (!ModelState.IsValid)
            return Page();

        var user = new ApplicationUser
        {
            UserName = Input.Username,
            Email = Input.Username,
            FullName = Input.FullName,
            EmailConfirmed = false
        };

        var result = await _userManager.CreateAsync(user, Input.Password);

        if (result.Succeeded)
        {
            await _userManager.AddClaimsAsync(user, new Claim[]
            {
                new(JwtClaimTypes.Email, Input.Username),
                new(JwtClaimTypes.Name, Input.FullName),
                new(JwtClaimTypes.Role, "user")
            });

            RegisterSuccess = true;

            return RedirectToPage("/Account/Register/Success", new { Email = user.Email });
        }

        ModelState.AddModelError("Error", "Не вдалось зареєструвати користувача. Будь ласка, перевірте введені дані та спробуйте ще раз");
        return Page();
    }
}