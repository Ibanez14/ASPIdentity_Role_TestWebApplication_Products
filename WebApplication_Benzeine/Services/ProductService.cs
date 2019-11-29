using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication_Benzeine.Data;
using WebApplication_Benzeine.Data.Models.DTO_s;
using WebApplication_Benzeine.Helpers;
using WebApplication_Benzeine.Models.ResponseDTO;

namespace WebApplication_Benzeine.Services
{
    /// <summary>
    /// Service that represent CRUD operations over Product and Category entites.
    /// I didn't separated services into ProductService and CategoryService, and didn't use RepositoryPattern
    /// All implemented together in one service.
    /// DTO not used
    /// </summary>
    public class ProductService : IProductService
    {
        DataContext dataContext;

        /// <summary>
        /// HttpAccessor to get information about User from
        /// </summary>
        private readonly IHttpContextAccessor accessor;

        // Lambda constructor
        public ProductService(DataContext dataContext, IHttpContextAccessor accessor) =>
                (this.dataContext, this.accessor) = (dataContext, accessor);
        

        /// <summary>
        /// Get all the products created by all users
        /// </summary>
        /// <returns></returns>
        public async Task<List<ProductResponseModel>> GetAllProducts()
        {
            return (await dataContext.Set<Product>()
                                 .Include(p => p.Category)
                                 .Include(p=>p.User)
                                 .ToListAsync())
                                 .MapToResponseModels()
                                 .ToList();
        }

        /// <summary>
        /// Get products that were created by Requested user
        /// </summary>
        /// <returns></returns>
        public async Task<List<ProductResponseModel>> GetUserProducts() =>
                (await dataContext.Set<Product>()
                                 .Where(p => p.UserId == accessor.HttpContext.User.FindFirst("UserID").Value)
                                 .Include(p => p.Category)
                                 .Include(p=>p.User)
                                 .ToListAsync())
                                 .MapToResponseModels()
                                 .ToList();


        public async Task<List<Category>> GetAllCategories() =>
                await dataContext.Set<Category>().ToListAsync();

        public async Task<ProductResponseModel> GetProductById(int id)
        {
            if (id < 0) return null;
            return (await dataContext.Set<Product>()
                                    .FindAsync(id))
                                    .MapToResponseModel();
        }

        /// <summary>
        /// Add a new Product.
        /// No need for validation since it will be validated in Controller
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public async Task<ProductResponseModel> AddProduct(ProductRequestModel dto)
        {
            // explicit convertion operator
            Product product = (Product)dto;

            var category = await dataContext.Set<Category>()
                                              .FirstOrDefaultAsync
                                              (c => c.Name.ToLower() == dto.CategoryName.ToLower());

            // Add Category if not exist
            if (category != null)
                product.CategoryId = category.Id;
            else
            {
                category = new Category() { Name = dto.CategoryName };
                dataContext.Add(category);
                await dataContext.SaveChangesAsync();
            }

            // Add Product
            product.CategoryId = category.Id;
            product.UserId = accessor.HttpContext.User.FindFirst("UserID").Value;
            dataContext.Set<Product>().Add(product);
            await dataContext.SaveChangesAsync();

            var responseModel = product.MapToResponseModel();
            responseModel.UserCreated = accessor.HttpContext.User.Identity.Name;

            return responseModel;
        }

        /// <summary>
        /// Delete Product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>Boolean indicating if product were succeffully deleted</returns>
        public async Task<(bool isDeleted, string ErrorMessage)> DeleteProductAsync(int productId)
        {
            var product = await dataContext.Set<Product>().FindAsync(productId);

            if (product != null)
            {
                // Check whether user is the same that created the product || if user is Admin
                if (product.UserId == accessor.HttpContext.User.FindFirst("UserID").Value
                    ||
                    accessor.HttpContext.User.IsInRole("Admin"))
                {
                    dataContext.Entry(product).State = EntityState.Deleted;
                    await dataContext.SaveChangesAsync();
                    return (true, string.Empty);
                }
                
                // otherwise user cannot delete
                return (false, "Sorry, but you have no right to delete the product that you didn't create");
            }
            return (false, "There is no such a product");
        }

    }
}
