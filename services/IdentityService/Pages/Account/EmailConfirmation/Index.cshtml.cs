using IdentityService.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;


namespace IdentityService.Pages.Account.EmailConfirmation;

[AllowAnonymous]
public class IndexModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;

    public bool IsSucceeded { get; set; }

    public IndexModel(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> OnGet(string userId, string token)
    {
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token))
        {
            IsSucceeded = false;
            ModelState.AddModelError("Error", "Посилання для підтвердження адреси електронної пошти є некоректним");
            return Page();
        }

        var user = await _userManager.FindByIdAsync(userId);
        if (user is null || user.EmailConfirmed)
        {
            IsSucceeded = false;
            ModelState.AddModelError("Error", "Ваш акаунт не було зареєстровано або адресу електронної пошти вже підтверджено");
            return Page();
        }

        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (result.Succeeded)
        {
            IsSucceeded = true;
            return Page();
        }

        IsSucceeded = false;
        ModelState.AddModelError("Error", "Не вдалось підтвердити електронну пошту. Будь ласка, спробуйте пізніше");
        return Page();
    }
}