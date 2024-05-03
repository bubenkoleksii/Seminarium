using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IdentityService.Pages.Account.Login;

public class InputModel
{
    [Required(ErrorMessage = "Це поле є обов'язковим")]
    [DisplayName("Адреса електронної пошти")]
    [EmailAddress(ErrorMessage = "Недійсний формат адреси електронної пошти")]
    public string? Username { get; set; }

    [Required(ErrorMessage = "Це поле є обов'язковим")]
    [DisplayName("Пароль")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль повинен містити принаймні {2} символів")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,}$",
        ErrorMessage = "Пароль повинен містити принаймні одну маленьку літеру, одну велику літеру, одну цифру та один спеціальний символ")]
    public string? Password { get; set; }

    [DisplayName("Запам'ятати мене")]
    public bool RememberLogin { get; set; }

    public string? ReturnUrl { get; set; }

    public string? ClientHomeUrl { get; set; }

    public string? Button { get; set; }
}