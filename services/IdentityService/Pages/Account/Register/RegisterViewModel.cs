using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace IdentityService.Pages.Account.Register;

public class RegisterViewModel
{
    [Required(ErrorMessage = "Це поле є обов'язковим")]
    [DisplayName("Пароль")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль повинен містити принаймні {2} символів")]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,}$",
        ErrorMessage = "Пароль повинен містити принаймні одну маленьку літеру, одну велику літеру, одну цифру та один спеціальний символ")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Це поле є обов'язковим")]
    [DisplayName("Повторно введіть пароль")]
    [Compare("Password", ErrorMessage = "Паролі повинні співпадати")]
    public string RepeatPassword { get; set; } = string.Empty;

    [Required(ErrorMessage = "Це поле є обов'язковим")]
    [DisplayName("Адреса електронної пошти")]
    [EmailAddress(ErrorMessage = "Недійсний формат адреси електронної пошти")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Це поле є обов'язковим")]
    [DisplayName("Повне ім'я")]
    public string FullName { get; set; } = string.Empty;

    [Required]
    [DisplayName("Ознайомлений із законом \"Про захист персональних даних\" та даю згоду на обробку даних (якщо вам немає 18 років, то узгодьте це з батьками)")]
    [Range(typeof(bool), "true", "true", ErrorMessage = "Для продовження необхідно погодитись із обробкою даних.")]
    public bool AgreeWithProcessing { get; set; }

    public string? ClientHomeUrl { get; set; }

    public string? Button { get; set; }
}