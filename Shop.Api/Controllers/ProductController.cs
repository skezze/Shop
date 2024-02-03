using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shop.Api.Models.ViewModels;
using Shop.Application.Services;
using Shop.Data;
using Shop.Domain.Models;

namespace Shop.Api.Controllers;
[ApiController, Route("api/[controller]/[action]")]
public class ProductController:ControllerBase
{
    private readonly ApplicationDbContext _context;
    private readonly ProductService _productService;

    public ProductController(ApplicationDbContext context, ProductService productService)
    {
        _context = context;
        _productService = productService;
    }
    [AllowAnonymous,HttpGet]
    public IActionResult GetProducts()
    {
       return Ok(_productService.GetProducts());
    }

    [AllowAnonymous,HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        var product = await _productService.GetProductById(id);
        
        if (product!=null)
        {
            return Ok(product);   
        }

        return NotFound(new{message="product id isn't finded"});
    }

    [Authorize(Policy = "Admin"),HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody]ProductView productView)
    {
        var product = new Product()
        {
            Title = productView.Title,
            Description = productView.Description,
            Price = productView.Price
        };
        var result = await _productService.CreateProduct(product);
        if (result!=null)
        {
            return Ok(result);
        }
        return BadRequest(new{message="product isn't created"});
    }

    [HttpDelete, Authorize(Policy = "Admin"), HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(int id)
    {
        var result = await _productService.DeleteProduct(id);
        return Ok(result);
    }
    
    [Authorize(Policy = "Admin"),HttpPut]
    public async Task<IActionResult> UpdateProduct([FromBody]Product product)
    {
        var result = await _productService.UpdateProduct(product);
        if (result==null)
        {
            return BadRequest(new{message="update is failed"});
        }

        return Ok(result);
    }
    
}