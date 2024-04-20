using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityService.Pages.Account.Register;

[AllowAnonymous]
public class SuccessModel : PageModel
{
    public required string Name { get; set; }

    public required string Email { get; set; }

    public SuccessModel(string name, string email)
    {
        Name = name;
        Email = email;
    }

    public void OnGet()
    {
    }
}