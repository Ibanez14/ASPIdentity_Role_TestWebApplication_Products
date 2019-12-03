using Microsoft.AspNetCore.Identity;
using System.Collections;
using System.Collections.Generic;

namespace WebApplication_Benzeine.Data.Models.Domain
{
    public class AppRole : IdentityRole<int>
    {
        public string Description { get; set; }
        public AppRole(string roleName) : base(roleName) { }
        public AppRole()
        {

        }
        public virtual ICollection<AppUserAppRole> UserRoles { get; set; }
    }
}
