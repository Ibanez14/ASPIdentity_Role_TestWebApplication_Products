using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication_Benzeine.Data.Models.DTO_s
{
    public class ProductRequestModel
    {
        [Required]
        public string Name { get; set; }

        [Range(0, double.MaxValue)]
        public int Price { get; set; }

        [Required]
        public string CategoryName { get; set; }

        public static explicit operator Product(ProductRequestModel dto)=> 
                    new Product()
                    {
                        Name = dto.Name,
                        Price = dto.Price,
                    };
        }
    }


