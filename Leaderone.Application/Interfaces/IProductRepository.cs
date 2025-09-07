using Leaderone.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leaderone.Application.Interfaces
{
    public interface IProductRepository
    {
        Task<bool> IsProductNameUniqueAsync(string name);
        Task<bool> IsProductNameUniqueAsync(Guid id, string name);

        Task<List<Product>?> GetAllProductsAsync(Guid tenantId);
        Task<Product?> GetProductAsync(Guid id);

        Task CreateProductAsync(Product product);
        Task UpdateProductAsync(Product product);
        Task DeleteProductAsync(Guid id);


    }
}
