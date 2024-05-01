using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Web;
using IdentityService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Shared.Utils.Mail;

namespace IdentityService.Pages.Account.ForgotPassword;

[AllowAnonymous]
public class IndexModel : PageModel
{
    private readonly IConfiguration _configuration;

    private readonly UserManager<ApplicationUser> _userManager;

    private readonly IMailService _mailService;

    public IndexModel(
        IConfiguration configuration,
        UserManager<ApplicationUser> userManager,
        IMailService mailService)
    {
        _configuration = configuration;
        _userManager = userManager;
        _mailService = mailService;
    }

    [BindProperty]
    [Required(ErrorMessage = "Це поле є обов'язковим")]
    [DisplayName("Адреса електронної пошти")]
    [EmailAddress(ErrorMessage = "Недійсний формат адреси електронної пошти")]
    public string Username { get; set; } = string.Empty;

    public string? ClientHomeUrl { get; set; }

    [BindProperty]
    public bool? IsSent { get; set; }

    public IActionResult OnGet()
    {
        ClientHomeUrl = _configuration["ClientHomeUrl"]!;
        return Page();
    }

    public async Task<IActionResult> OnPost()
    {
        ClientHomeUrl = _configuration["ClientHomeUrl"]!;

        if (!ModelState.IsValid)
            return Page();

        if (!string.IsNullOrEmpty(Username))
        {
            try
            {
                await SendResetEmail();
            }
            catch
            {
                IsSent = false;

                ModelState.AddModelError("Error", "Не вдалось надіслати лист. Будь ласка, спробуйте ще раз пізніше");
                Serilog.Log.Error($"Reset email not sent for user with email: {Username}");
                throw;
            }
        }

        return Page();
    }

    private async Task SendResetEmail()
    {
        IsSent = true;

        var user = await _userManager.FindByNameAsync(Username);
        if (user is null || !user.EmailConfirmed)
            return;

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
        var resetPasswordLink = $"{baseUrl}/Account/ForgotPassword/Reset?UserId={HttpUtility.UrlEncode(user.Id)}&Token={HttpUtility.UrlEncode(token)}";

        await _mailService.SendAsync("bubenkooleksii@gmail.com", EmailTemplate.Subject, EmailTemplate.GetBodyWithResetLink(resetPasswordLink));

        Serilog.Log.Information($"Reset password email was sent for user with email: {user.Email}");
    }

    protected class EmailTemplate
    {
        protected internal static string Subject => "Відновлення паролю на платформі Seminarium";

        protected internal static string GetBodyWithResetLink(string resetLink)
        {
            return $@"<div style='max-width: 600px; margin: 0 auto; padding: 20px; background-color: #fff; border-radius: 5px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);'>
            <h1 style='color: #333; text-align: center;'>Відновлення паролю на платформі Seminarium</h1>
            <p style='font-family: Arial, sans-serif;'>Ви запросили відновлення паролю на платформі Seminarium. Щоб встановити новий пароль, перейдіть за посиланням нижче:</p>
            <p style='text-align: center;'>
                <a href='{resetLink}' style='display: inline-block; background-color: #888; color: #fff; padding: 10px 20px; text-decoration: none; border-radius: 5px;'>Відновити пароль</a>
            </p>
            <p style='font-family: Arial, sans-serif;'>Якщо ви не здійснювали запит на відновлення паролю, проігноруйте це повідомлення.</p>
        </div>";
        }
    }
}
