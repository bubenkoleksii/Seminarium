using System.ComponentModel.DataAnnotations;

namespace IdentityService.Pages.Account.Register;

public class RegisterViewModel
{
    [Required] public string Password { get; set; } = string.Empty;

    [Required] public string Username { get; set; } = string.Empty;

    [Required] public string FullName { get; set; } = string.Empty;

    public string? ReturnUrl { get; set; }

    public string? Button { get; set; }
}