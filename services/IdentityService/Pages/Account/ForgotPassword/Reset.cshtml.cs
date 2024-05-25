using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using IdentityService.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityService.Pages.Account.ForgotPassword;

[AllowAnonymous]
public class ResetModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;

    private readonly IConfiguration _configuration;

    private readonly SignInManager<ApplicationUser> _signInManager;

    public ResetModel(
        IConfiguration configuration,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager)
    {
        _configuration = configuration;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    public string? ClientHomeUrl { get; set; }

    [BindProperty]
    [Required(ErrorMessage = "Це поле є обов'язковим")]
    [DisplayName("Пароль")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль повинен містити принаймні {2} символів")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,}$",
        ErrorMessage = "Пароль повинен містити принаймні одну маленьку літеру, одну велику літеру, одну цифру та один спеціальний символ")]
    public string Password { get; set; } = string.Empty;

    [BindProperty]
    [Required(ErrorMessage = "Це поле є обов'язковим")]
    [DisplayName("Повторно введіть пароль")]
    [Compare("Password", ErrorMessage = "Паролі повинні співпадати")]
    public string RepeatPassword { get; set; } = string.Empty;

    [BindProperty] public string Token { get; set; }

    [BindProperty] public string UserId { get; set; }

    [BindProperty] public bool IsTokenValid { get; set; }

    public async Task<IActionResult> OnGet(string userId, string token)
    {
        Token = token;
        UserId = userId;
        ClientHomeUrl = _configuration["ClientHomeUrl"]!;

        var user = await _userManager.FindByIdAsync(UserId);
        if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(token) ||
            user is null || !user.EmailConfirmed)
        {
            IsTokenValid = false;
        }
        else
            IsTokenValid = await _userManager
                .VerifyUserTokenAsync(user, _userManager.Options.Tokens.PasswordResetTokenProvider, "ResetPassword", token);

        if (!IsTokenValid)
        {
            ModelState.AddModelError("Error", "Посилання для відновлення паролю є некоректним");
        }

        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        var user = await _userManager.FindByIdAsync(UserId);

        if (!ModelState.IsValid || user is null ||
            string.IsNullOrEmpty(UserId) || string.IsNullOrEmpty(Token))
            return Page();

        var result = await _userManager.ResetPasswordAsync(user, Token, Password);
        if (result.Succeeded)
        {
            await _signInManager.SignOutAsync();

            Serilog.Log.Error($"Reset password and sign out were success: {user.Email}");
            return RedirectToPage("/Account/ForgotPassword/Success", new { user.Email });
        }

        Serilog.Log.Error($"Reset password not success: {user.Email}");
        ModelState.AddModelError("Error", "Не вдалось змінити пароль");
        return Page();
    }
}