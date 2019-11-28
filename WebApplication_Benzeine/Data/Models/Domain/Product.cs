using Microsoft.AspNetCore.Identity;

namespace WebApplication_Benzeine.Data
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        // Nav properties
        public int CategoryId { get; set; }
        public Category Category { get; set; }

        // This is not Foreign key
        public string UserId { get; set; }
    }
}
