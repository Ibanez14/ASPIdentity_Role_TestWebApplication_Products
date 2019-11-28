using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication_Benzeine.Data.Models.DTO_s
{
    public class ProductDTO
    {
        [Required]
        public string Name { get; set; }

        [MinLength(1)]
        public int Price { get; set; }

        [Required]
        public string CategoryName { get; set; }

        public static explicit operator Product(ProductDTO dto)
        {
            return new Product()
            {
                Name = dto.Name,
                Price = dto.Price,
            };
        }
    }

}
