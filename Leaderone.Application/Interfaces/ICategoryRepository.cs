using Leaderone.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leaderone.Application.Interfaces
{
    public interface ICategoryRepository
    {
        Task<bool> IsCategoryNameUniqueAsync(string name);
        Task<bool> IsCategoryNameUniqueAsync(Guid id, string name);
        Task<List<Category>?> GetAllCategoriesAsync(Guid tenantId);
        Task<Category?> GetCategoryAsync(Guid categoryId);

        Task AddCategoryAsync(Category category);
        Task UpdateCategoryAsync(Category category);
        Task DeleteCategoryAsync(Guid categoryId);



    }
}
