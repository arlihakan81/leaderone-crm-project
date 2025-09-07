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
    public class CategoryRepository(LeaderoneDbContext context) : ICategoryRepository
    {
        private readonly LeaderoneDbContext _context = context;

        public async Task AddCategoryAsync(Category category)
        {
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCategoryAsync(Guid categoryId)
        {
            _context.Categories.Remove(_context.Categories.FirstOrDefault(c => c.Id == categoryId)!);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Category>?> GetAllCategoriesAsync(Guid tenantId)
        {
            return await _context.Categories.Where(c => c.TenantId == tenantId).ToListAsync();
        }

        public async Task<Category?> GetCategoryAsync(Guid categoryId)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);
        }

        public async Task<bool> IsCategoryNameUniqueAsync(string name)
        {
            return await _context.Categories.AnyAsync(c => c.Name.ToLower().Trim() != name.ToLower().Trim());
        }

        public async Task<bool> IsCategoryNameUniqueAsync(Guid id, string name)
        {
            return await _context.Categories.AnyAsync(c => c.Id != id && c.Name.ToLower().Trim() == name.ToLower().Trim());
        }

        public async Task UpdateCategoryAsync(Category category)
        {
            _context.Categories.Update(category);
            await _context.SaveChangesAsync();
        }
    }
}
