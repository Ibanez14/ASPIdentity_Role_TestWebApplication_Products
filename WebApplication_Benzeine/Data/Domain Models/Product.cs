using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using WebApplication_Benzeine.Data.Models.Domain;

namespace WebApplication_Benzeine.Data
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation properties
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }


        public int UserId { get; set; }
        public virtual AppUser User { get; set; }
    }
}
