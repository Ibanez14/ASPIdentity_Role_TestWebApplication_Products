using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication_Benzeine.Data;
using WebApplication_Benzeine.Data.Models.DTO_s;
using WebApplication_Benzeine.Models.ResponseDTO;
using WebApplication_Benzeine.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebApplication_Benzeine.Controller
{
    [ApiController]
    [Route("api/v1/[controller]/[action]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProductController : ControllerBase
    {
        private readonly IProductService productService;
        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        /// <summary>
        ///  Return all products created by users
        /// </summary>
        /// <returns>List of product</returns>
        /// <response code="200">Success. Return a List of product</response>
        /// <response code="404">Not Found. Returns nothing if there is no products</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<List<ProductResponseModel>>> GetAllProducts()
        {
            var products = await productService.GetAllProducts();
            
            if (products == null)
                return NotFound();

            return products;
        }


        /// <summary>
        ///  Return only user-created products 
        /// </summary>
        /// <returns>List of product</returns>
        /// <response code="200">Success. Return a List of product</response>
        /// <response code="404">Not Found. Returns nothing if there is no products</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<List<ProductResponseModel>>> GetUserProducts()
        {
            var products = await productService.GetUserProducts();

            if (products == null)
                return NotFound();

            return products;
        }

        /// <summary>
        ///  Return all categories related with products
        /// </summary>
        /// <returns>List of categories</returns>
        /// <response code="200">Success. Return a List of category</response>
        /// <response code="404">Not Found. Returns nothing if there is no category</response>
        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<ActionResult<List<Category>>> GetCategories()
        {
            var categories = await productService.GetAllCategories();
            if (categories == null)
                return NotFound();

            return categories;
        }

        /// <summary>
        ///  Return the category based on specified ID parameter
        /// </summary>
        /// <returns>Category</returns>
        /// <response code="200">Success. Return a requested category</response>
        /// <response code="404">Not Found. Returns nothing if there is no category with provided ID</response>
        /// <response code="400">Bad Request. If ID wasn't valid (Example ID<0)</response>
        [HttpGet]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetCatogoryByID(int id)
        {
            if (id < 0)
                return BadRequest("Adam kimi ID gonderin");

            var product = await productService.GetProductById(id);
            if (product == null)
                return NotFound();

            return Ok(product);
        }

        /// <summary>
        ///  Add product to the database
        /// </summary>
        /// <returns>Category</returns>
        /// <response code="201">Created. Product added</response>
        /// <response code="400">Bad Request. Product model wasn't valid</response>
        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        // ProductDTO is validate in ApiController attribute
        public async Task<ActionResult<ProductResponseModel>> AddProduct(ProductRequestModel dto)
        {
            var product = await productService.AddProduct(dto);
            return CreatedAtAction(nameof(AddProduct), product);
        }


        /// <summary>
        /// And Remember! Only User created this product can delete it, otherwise user will be denied. DEMOCRATIC !
        /// </summary>
        /// <param name="id">ID parameter</param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (id < 0) 
                return BadRequest("Adam kimi ID gonderin");

            (bool isDeleted, string errorMessage) = await productService.DeleteProductAsync(productId: id);

            if (isDeleted)
                return NoContent();

            return BadRequest(errorMessage + ". God Bless You");
        }
    }
}
