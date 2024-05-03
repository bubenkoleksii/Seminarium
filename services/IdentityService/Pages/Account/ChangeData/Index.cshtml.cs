using IdentityService.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityService.Pages.Account.ChangeData;

[Authorize]
public class IndexModel : PageModel
{
    private readonly UserManager<ApplicationUser> _userManager;

    private readonly SignInManager<ApplicationUser> _signInManager;

    private readonly IConfiguration _configuration;

    public IndexModel(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    [BindProperty]
    public ChangeDataViewModel Input { get; set; }

    [BindProperty]
    public bool IsValid { get; set; }

    [BindProperty]
    public bool IsSucceeded { get; set; }

    [BindProperty]
    public bool IsNeedLogin { get; set; }

    public async Task<IActionResult> OnGet()
    {
        try
        {
            Input = new ChangeDataViewModel
            {
                ClientHomeUrl = _configuration["ClientHomeUrl"]!
            };

            var user = await _userManager.GetUserAsync(User);
            if (user is not { EmailConfirmed: true })
            {
                IsValid = false;
                ModelState.AddModelError("Error", "To change account data, confirm your email address.");
                Serilog.Log.Information("Email address is not confirmed.");
                return Page();
            }

            IsValid = true;
            Input.Username = user.UserName!;
            Input.FullName = user.FullName;

            return Page();
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "Error in change data: {Message}", ex.Message);
            throw;
        }
    }

    public async Task<IActionResult> OnPost()
    {
        try
        {
            var clientHomeUrl = _configuration["ClientHomeUrl"]!;
            Input.ClientHomeUrl = clientHomeUrl;

            if (Input.Button != "change-data")
                return Redirect(clientHomeUrl);

            if (!ModelState.IsValid)
                return Page();

            var user = await _userManager.GetUserAsync(User);
            if (user is not { EmailConfirmed: true })
            {
                IsValid = false;
                ModelState.AddModelError("Error", "Для того, щоб змінити дані акаунту, підтвердіть адресу електроної пошти.");
                Serilog.Log.Information("Error in change data method: Email address is not confirmed.");
                return Page();
            }

            if (IsFullNameChanged(Input.FullName, user))
                user.FullName = Input.FullName;

            if (IsPasswordChanged(Input.Password, Input.OldPassword))
            {
                var changePasswordResult = await _userManager.ChangePasswordAsync(user, Input.OldPassword!, Input.Password!);
                if (!changePasswordResult.Succeeded)
                {
                    IsSucceeded = false;
                    ModelState.AddModelError("Error", "Не вдалося змінити пароль. Перевірте вхідні дані та повторіть спробу пізніше");
                    Serilog.Log.Error("Error in change data method while updating user: Failed to update password.");
                    return Page();
                }

                await _signInManager.SignOutAsync();
                IsNeedLogin = true;
            }

            try
            {
                if (IsUserNameChanged(Input.Username, user))
                {
                    await _userManager.SetUserNameAsync(user, Input.Username);
                    await _userManager.SetEmailAsync(user, Input.Username);

                    Serilog.Log.Information("User's email was changed: {email}. New email: {NewEmail}", user.Email, Input.Username);

                    await _signInManager.SignOutAsync();
                    IsNeedLogin = true;
                }
            }
            catch (Exception ex)
            {
                Serilog.Log.Error(ex, "Error in change data method while changing username and email: {Message}", ex.Message);

                IsSucceeded = false;
                ModelState.AddModelError("Error", "Не вдалося змінити дані облікового запису. Перевірте вхідні дані та повторіть спробу пізніше");
                return Page();
            }

            var updateResult = await _userManager.UpdateAsync(user);
            if (!updateResult.Succeeded)
            {
                IsSucceeded = false;
                ModelState.AddModelError("Error", "Не вдалося змінити дані облікового запису. Перевірте вхідні дані та повторіть спробу пізніше");
                Serilog.Log.Error("Error in change data method while updating user: Failed to update account data.");
                return Page();
            }

            IsSucceeded = true;
            Serilog.Log.Information("Change data method executed successfully. Changes to the account data were successfully saved.");
            return Page();
        }
        catch (Exception ex)
        {
            Serilog.Log.Error(ex, "Error in change data method: {Message}", ex.Message);
            throw;
        }
    }

    private bool IsFullNameChanged(string fullName, ApplicationUser user) =>
        !string.Equals(fullName, user.FullName, StringComparison.CurrentCultureIgnoreCase);

    private bool IsUserNameChanged(string userName, ApplicationUser user) =>
        !string.Equals(userName, user.NormalizedUserName, StringComparison.CurrentCultureIgnoreCase);

    private bool IsPasswordChanged(string? password, string? oldPassword) =>
        !string.IsNullOrWhiteSpace(password) && !string.IsNullOrWhiteSpace(oldPassword);
}
