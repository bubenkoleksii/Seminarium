using Microsoft.AspNetCore.Identity;

namespace IdentityService.Models;

public class ApplicationUser : IdentityUser
{
    public required string FullName { get; set; }
}
