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
        ///  Props doesnt work
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<List<Product>>> GetProducts()
        {
            var products = await productService.GetAllProducts();

            if (products == null)
                return NotFound();

            return products;
        }


        [HttpGet]
        [ProducesResponseType(404)]
        [ProducesResponseType(200)]
        public async Task<ActionResult<List<Category>>> GetCategories()
        {

            var categories = await productService.GetAllCategories();
            if (categories == null)
                return NotFound();

            return categories;
        }


        [HttpGet]
        [ProducesResponseType(404)]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetCatogory(int id)
        {
            if (id < 0)
                return BadRequest("Adam kimi ID gonderin");

            var product = await productService.GetProductById(id);
            if (product == null)
                return NotFound();

            return product;
        }

        [HttpPost]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        // ProductDTO is validate in ApiController attribute
        public async Task<ActionResult<Product>> AddProduct(ProductDTO dto)
        {
            var product = await productService.AddProduct(dto, User.FindFirst("id").Value);
            return CreatedAtAction(nameof(AddProduct), product);
        }


        /// <summary>
        /// And Remember! Only User created this product can delete it, othere users will be denie. DEMOCRATIC!
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            if (id < 0) 
                return BadRequest("Adam kimi ID gonderin");

            (bool deleted, string errorMessage) = 
                await productService.DeleteProduct(productId: id,
                                                      userId: User.FindFirst("id").Value);
            if (deleted)
                return NoContent();

            return BadRequest(errorMessage.Concat(". God Bless You"));
        }
    }
}
