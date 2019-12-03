using Microsoft.AspNetCore.Identity;

namespace WebApplication_Benzeine.Data.Models.Domain
{
    public class AppUserAppRole:IdentityUserRole<int>
    {
        public virtual AppUser User { get; set; }
        public virtual AppRole Role { get; set; }
    }
}
