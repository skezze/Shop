using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Domain.Models;

namespace Shop.Application.Services;

public class ProductService
{
    private readonly ApplicationDbContext _dbContext;

    public ProductService(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public List<Product> GetProducts()
    {
        return _dbContext.Products.ToList();
    }

    public async Task<Product?> GetProductById( int id)
    {
        var product = await _dbContext.Products.
            FirstOrDefaultAsync(x => x.Id == id);
        if (product!=null)
        {
            return product;   
        }
        return null;
    }

    public async Task<Product?> CreateProduct(Product product)
    {
       if (product.Title!=string.Empty&&product.Description!=string.Empty&&product.Price>0)
       {
            var result = await _dbContext.Products.AddAsync(product);
            await _dbContext.SaveChangesAsync();
            return product;
       }
       return null;
    }

    public async Task<bool> DeleteProduct(int id)
    {
        var product = await _dbContext.Products.FirstOrDefaultAsync(x => x.Id == id);
        if (product!=null)
        {
            _dbContext.Remove(product);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        return false;
    }
    
    public async Task<Product?> UpdateProduct(Product product)
    {
        if (product.Id>0&&product.Title!=""&&product.Description!=""&&product.Price>0)
        {
            var productInDb = await _dbContext.Products.FirstOrDefaultAsync(x=>x.Id==product.Id);
            if (productInDb!=null)
            {
                productInDb.Description = product.Description;
                productInDb.Title = product.Title;
                productInDb.Price = product.Price;
                await _dbContext.SaveChangesAsync();
                return productInDb;
            }
        }
        return null;
    }
}