using System.Collections.Generic;
using System.Threading.Tasks;
using WebApplication_Benzeine.Data;
using WebApplication_Benzeine.Data.Models.DTO_s;
using WebApplication_Benzeine.Models.ResponseDTO;

namespace WebApplication_Benzeine.Services
{
    /// <summary>
    /// Interface that is responsible to add, get and remove product
    /// </summary>
    public interface IProductService
    {
        Task<ProductResponseModel> AddProduct(ProductRequestModel dto);
        Task<(bool isDeleted, string ErrorMessage)> DeleteProductAsync(int productId);
        Task<List<Category>> GetAllCategories();
        Task<List<ProductResponseModel>> GetAllProducts();
        Task<ProductResponseModel> GetProductById(int id);
        Task<List<ProductResponseModel>> GetUserProducts();
    }
}