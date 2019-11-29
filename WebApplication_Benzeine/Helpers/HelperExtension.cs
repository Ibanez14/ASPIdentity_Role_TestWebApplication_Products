using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApplication_Benzeine.Data;
using WebApplication_Benzeine.Models.ResponseDTO;

namespace WebApplication_Benzeine.Helpers
{
    public static class HelperExtension
    {
        /// <summary>
        /// Maps Product collection to ProductResponseModel collection
        /// </summary>
        /// <param name="products"></param>
        /// <returns></returns>
        public static IEnumerable<ProductResponseModel> MapToResponseModels(this IEnumerable<Product> products)
        {
            foreach (var item in products)
                yield return new ProductResponseModel
                {
                    Id = item.Id,
                    CategoryName = item.Category.Name,
                    Price = item.Price,
                    Name = item.Name,
                    UserCreated = item.User.UserName
                };
            
        }

        /// <summary>
        /// Maps Product to ProductResponseModel 
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public static ProductResponseModel MapToResponseModel(this Product product)
        {
            return new ProductResponseModel
            {
                Id = product.Id,
                CategoryName = product?.Category?.Name,
                Price = product.Price,
                Name = product.Name
            };
        } 

        /// <summary>
        /// Convert Claim collectin to Dictionary with Key: ClaimType and Value: ClaimValu
        /// </summary>
        /// <param name="claims"></param>
        /// <returns></returns>
        public static IDictionary<string,string> ToDictionary(this IEnumerable<Claim> claims)
        {
            var claimTypeValue = new Dictionary<string, string>();

            foreach (var item in claims)
                claimTypeValue.Add(item.Type, item.Value);

            return claimTypeValue;
        }
    }
}
