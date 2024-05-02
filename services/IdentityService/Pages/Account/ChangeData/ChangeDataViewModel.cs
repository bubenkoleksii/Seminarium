using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IdentityService.Pages.Account.ChangeData;

public class ChangeDataViewModel
{
    [DisplayName("Новий пароль")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль повинен містити принаймні {2} символів")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,}$",
    ErrorMessage = "Пароль повинен містити принаймні одну маленьку літеру, одну велику літеру, одну цифру та один спеціальний символ")]
    public string? Password { get; set; } = string.Empty;

    [DisplayName("Старий пароль")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль повинен містити принаймні {2} символів")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,}$",
        ErrorMessage = "Пароль повинен містити принаймні одну маленьку літеру, одну велику літеру, одну цифру та один спеціальний символ")]
    public string? OldPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Це поле є обов'язковим")]
    [DisplayName("Адреса електронної пошти")]
    [EmailAddress(ErrorMessage = "Недійсний формат адреси електронної пошти")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Це поле є обов'язковим")]
    [DisplayName("Повне ім'я")]
    public string FullName { get; set; } = string.Empty;

    public string? ClientHomeUrl { get; set; }

    public string? Button { get; set; }
}
