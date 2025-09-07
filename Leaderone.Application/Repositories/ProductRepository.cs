using Leaderone.Application.Interfaces;
using Leaderone.Domain.Entities;
using Leaderone.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leaderone.Application.Repositories
{
    public class ProductRepository(LeaderoneDbContext context) : IProductRepository
    {
        private readonly LeaderoneDbContext _context = context;

        public async Task CreateProductAsync(Product product)
        {
            _context.Products.Add(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProductAsync(Guid id)
        {
            _context.Products.Remove(_context.Products.FirstOrDefault(p => p.Id == id)!);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Product>?> GetAllProductsAsync(Guid tenantId)
        {
            return await _context.Products.Where(p => p.TenantId == tenantId).ToListAsync();
        }

        public async Task<Product?> GetProductAsync(Guid id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> IsProductNameUniqueAsync(string name)
        {
            return await _context.Products.AnyAsync(p => p.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public async Task<bool> IsProductNameUniqueAsync(Guid id, string name)
        {
            return await _context.Products.Where(p => p.Id != id).AnyAsync(p => p.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public async Task UpdateProductAsync(Product product)
        {
            _context.Products.Update(product);
            await _context.SaveChangesAsync();
        }
    }
}
