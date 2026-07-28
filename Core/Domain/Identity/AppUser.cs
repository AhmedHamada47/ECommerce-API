using Microsoft.AspNetCore.Identity;

namespace Domain.Identity
{
    public class AppUser : IdentityUser
    {
        public string DisplayName { get; set; } = default!;
    }
}
