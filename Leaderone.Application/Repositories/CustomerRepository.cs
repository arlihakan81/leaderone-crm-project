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
    public class CustomerRepository(LeaderoneDbContext context) : ICustomerRepository
    {
        private readonly LeaderoneDbContext _context = context;

        public async Task AddCustomerAsync(Customer customer)
        {
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCustomerAsync(Guid id)
        {
            var customer = _context.Customers.Find(id);
            if (customer != null)
            {
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Customer>?> GetAllCustomersAsync(Guid tenantId)
        {
            return await _context.Customers
                .Where(customer => customer.TenantId == tenantId)
                .ToListAsync();
        }

        public async Task<Customer?> GetCustomerByEmailAsync(string email)
        {
            return await _context.Customers.FirstOrDefaultAsync(customer => customer.Email == email);
        }

        public async Task<Customer?> GetCustomerByIdAsync(Guid id)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> IsExistsCustomerEmailAsync(string email)
        {
            return await _context.Customers.AnyAsync(customer => customer.Email.ToLower().Trim() == email.ToLower().Trim());
        }

        public async Task<bool> IsExistsCustomerEmailAsync(Guid id, string email)
        {
            return await _context.Customers
                .AnyAsync(customer => customer.Id != id && customer.Email.ToLower().Trim() == email.ToLower().Trim());
        }

        public async Task<bool> IsExistsCustomerPhoneAsync(string phone)
        {
            return await _context.Customers.AnyAsync(customer => customer.Phone.ToLower().Trim() == phone.ToLower().Trim());
        }

        public async Task<bool> IsExistsCustomerPhoneAsync(Guid id, string phone)
        {
            return await _context.Customers
                .AnyAsync(customer => customer.Id != id && customer.Phone.ToLower().Trim() == phone.ToLower().Trim());
        }

        public async Task UpdateCustomerAsync(Customer customer)
        {
            _context.Customers.Update(customer);
            await _context.SaveChangesAsync();
        }
    }
}
