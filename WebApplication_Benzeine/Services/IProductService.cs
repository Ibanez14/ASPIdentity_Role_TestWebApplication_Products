using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication_Benzeine.Data;
using WebApplication_Benzeine.Data.Models.DTO_s;

namespace WebApplication_Benzeine.Services
{
    public interface IProductService
    {
        Task<Product> AddProduct(ProductDTO dto, string userId);
        Task<(bool isDeleted, string ErrorMessage)> DeleteProduct(int productId, string userId);
        Task<List<Category>> GetAllCategories();
        Task<List<Product>> GetAllProducts();
        Task<Product> GetProductById(int id);
    }
}