using Leaderone.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leaderone.Application.Interfaces
{
    public interface ICustomerRepository
    {
        Task<List<Customer>?> GetAllCustomersAsync(Guid tenantId);
        Task<Customer?> GetCustomerByEmailAsync(string email);

        Task<Customer?> GetCustomerByIdAsync(Guid id);

        Task<bool> IsExistsCustomerEmailAsync(string email);
        Task<bool> IsExistsCustomerEmailAsync(Guid id, string email);

        Task<bool> IsExistsCustomerPhoneAsync(string phone);
        Task<bool> IsExistsCustomerPhoneAsync(Guid id, string phone);

        Task AddCustomerAsync(Customer customer);
        Task UpdateCustomerAsync(Customer customer);
        Task DeleteCustomerAsync(Guid id);

    }
}
