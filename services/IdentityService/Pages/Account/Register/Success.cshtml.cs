using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityService.Pages.Account.Register;

[AllowAnonymous]
public class SuccessModel : PageModel
{
    public required string Email { get; set; }

    public void OnGet(string email)
    {
        Email = email;
    }
}