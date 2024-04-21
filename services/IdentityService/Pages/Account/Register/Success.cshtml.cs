using IdentityService.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

using Shared.Utils.Mail;

namespace IdentityService.Pages.Account.Register;

[AllowAnonymous]
public class SuccessModel : PageModel
{
    public required string Email { get; set; }

    private readonly IMailService _mailService;

    private readonly UserManager<ApplicationUser> _userManager;

    public SuccessModel(IMailService mailService, UserManager<ApplicationUser> userManager)
    {
        _mailService = mailService;
        _userManager = userManager;
    }

    public void OnGet(string email)
    {
        Email = email;

        _mailService.SendAsync("ipz202_bov@student.ztu.edu.ua", EmailTemplate.Subject, "<b>Text 2</b>");
    }

    private async Task SendConfirmationEmail()
    {
        var user = await _userManager.FindByNameAsync(Email);
        if (user is null || user.EmailConfirmed)
            return;

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);


        await _mailService.SendAsync("ipz202_bov@student.ztu.edu.ua", EmailTemplate.Subject, "<b>Text 2</b>");
    }

    protected class EmailTemplate
    {
        protected internal static string Subject => "Підтвердження реєстрації на платформі Seminarium";
    }
}