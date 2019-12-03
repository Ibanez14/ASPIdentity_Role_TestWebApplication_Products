using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication_Benzeine.Data.Models.Domain
{
    public class AppUser : IdentityUser<int>
    {
        public virtual IEnumerable<Product> Products { get; set; }

        public virtual ICollection<IdentityUserClaim<int>> Claims { get; set; }
        public virtual ICollection<IdentityUserLogin<int>> Logins { get; set; }
        public virtual ICollection<IdentityUserToken<int>> Tokens { get; set; }
        
        // Many To Many
        public virtual ICollection<AppUserAppRole> UserRoles { get; set; }
    }
}
