using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication_Benzeine.Models.RequestDTO
{
    // This object is used as a DTO for registration and login
    /// <summary>
    /// REquest Model
    /// </summary>
    public class RequestModel
    {
        /// <summary>
        /// Email Address should be in correct way.
        /// benzeine.com domains can have access  api/wifi URL that return wifi password
        /// </summary>
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [MinLength(8)]
        public string Password { get; set; }
    }
}
