using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication_Benzeine.Data;
using WebApplication_Benzeine.Data.Models.DTO_s;

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
        public ProductService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<List<Product>> GetAllProducts() =>
                await dataContext.Set<Product>()
                                 .Include(p => p.Category)
                                 .ToListAsync();

        public async Task<List<Category>> GetAllCategories() =>
                await dataContext.Set<Category>().ToListAsync();

        public async Task<Product> GetProductById(int id)
        {
            if (id < 0) return null;
            return await dataContext.Set<Product>().FindAsync(id);
        }

        /// <summary>
        /// Add a new Product.
        /// No need for validation since it will be validated in Controller
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>

        public async Task<Product> AddProduct(ProductDTO dto, string userId)
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
                dataContext.Set<Category>()
                           .Add(category);
                await dataContext.SaveChangesAsync();
            }

            // Add Product
            product.CategoryId = category.Id;
            product.UserId = userId;

            dataContext.Set<Product>().Add(product);
            await dataContext.SaveChangesAsync();
            return product;
        }

        /// <summary>
        /// Delete Product
        /// </summary>
        /// <param name="productId"></param>
        /// <returns>Boolean indicating if product were succeffully deleted</returns>
        public async Task<(bool isDeleted, string ErrorMessage)> DeleteProduct(int productId, string userId)
        {
            var product = await dataContext.Set<Product>().FindAsync(productId);

            // if there is such a user with this id, we delete it otherwise not
            if (product != null)
            {
                // Only user that created the  product can delete it
                if (product.UserId == userId)
                {
                    dataContext.Set<Product>().Remove(new Product() { Id = productId });
                    await dataContext.SaveChangesAsync();
                    return (true, string.Empty);
                }
                // otherwise user cannot delete
                return (false, "Sorry, but you have no right to delete the product that you didn't create");
            }
            return (false, "There is no such a user");
        }

    }
}
