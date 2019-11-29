using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication_Benzeine.Data.Models.Domain
{
    public class ApplicationUser:IdentityUser
    {
        public IEnumerable<Product> Products { get; set; }
    }
}
