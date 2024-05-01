using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityService.Pages.Account.ForgotPassword;

[AllowAnonymous]
public class ResetModel : PageModel
{
    private readonly IConfiguration _configuration;

    public ResetModel(IConfiguration configuration)
    {
        _configuration = configuration;
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

    public void OnGet()
    {
        ClientHomeUrl = _configuration["ClientHomeUrl"]!;
    }
}