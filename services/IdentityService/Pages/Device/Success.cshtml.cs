using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityService.Pages.Device;

[SecurityHeaders]
[Authorize(Roles = "Admin")]
public class SuccessModel : PageModel
{
    public void OnGet()
    {
    }
}
