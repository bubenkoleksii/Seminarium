using System.Web;

using IdentityService.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Shared.Utils.Mail;

namespace IdentityService.Pages.Account.Register;

[AllowAnonymous]
public class SuccessModel : PageModel
{
    [BindProperty]
    public required string Email { get; set; }

    public bool IsResent { get; set; }

    private readonly IMailService _mailService;

    private readonly UserManager<ApplicationUser> _userManager;

    public SuccessModel(IMailService mailService, UserManager<ApplicationUser> userManager)
    {
        _mailService = mailService;
        _userManager = userManager;
    }

    public async Task OnGet(string email)
    {
        Email = email;

        if (!string.IsNullOrEmpty(email))
        {
            try
            {
                await SendConfirmationEmail();
            }
            catch
            {
                ModelState.AddModelError("Error", "Не вдалось надіслати лист. Будь ласка, спробуйте ще раз пізніше");
                Serilog.Log.Error($"Confirmation email not sent for user with email: {email}");
                throw;
            }
        }
    }

    public async Task OnPost()
    {
        if (!string.IsNullOrEmpty(Email))
        {
            try
            {
                await SendConfirmationEmail();
                IsResent = true;
            }
            catch
            {
                IsResent = false;

                ModelState.AddModelError("Error", "Не вдалось надіслати лист. Будь ласка, спробуйте ще раз пізніше");
                Serilog.Log.Error($"Confirmation email not sent for user with email: {Email}");
                throw;
            }
        }
    }

    private async Task SendConfirmationEmail()
    {
        var user = await _userManager.FindByNameAsync(Email);
        if (user is null || user.EmailConfirmed)
            return;

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        var baseUrl = $"{Request.Scheme}://{Request.Host}{Request.PathBase}";
        var confirmationLink = $"{baseUrl}/Account/EmailConfirmation?UserId={HttpUtility.UrlEncode(user.Id)}&Token={HttpUtility.UrlEncode(token)}";

        await _mailService.SendAsync(Email, EmailTemplate.Subject, EmailTemplate.GetBodyWithConfirmationLink(confirmationLink));
        Serilog.Log.Information($"Confirmation email was sent for user with email: {user.Email}");
    }

    protected class EmailTemplate
    {
        protected internal static string Subject => "Підтвердження реєстрації на платформі Seminarium";

        protected internal static string GetBodyWithConfirmationLink(string confirmationLink)
        {
            return $@"<div style='max-width: 600px; margin: 0 auto; padding: 20px; background-color: #fff; border-radius: 5px; box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);'>
            <h1 style='color: #333; text-align: center;'>Будь ласка, підтвердіть реєстрацію на платформі Seminarium</h1>
            <p style='font-family: Arial, sans-serif;'>Вітаємо! Дякуємо за реєстрацію на платформі Seminarium. Щоб завершити процес реєстрації, будь ласка, підтвердіть свою електронну адресу, перейшовши за посиланням нижче:</p>
            <p style='text-align: center;'>
                <a href='{confirmationLink}' style='display: inline-block; background-color: #007bff; color: #fff; padding: 10px 20px; text-decoration: none; border-radius: 5px;'>Підтвердити електронну пошту</a>
            </p>
            <p style='font-family: Arial, sans-serif;'>Якщо ви не реєструвалися на платформі Seminarium, проігноруйте це повідомлення.</p>
        </div>";
        }
    }
}