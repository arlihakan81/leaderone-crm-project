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
    public class AppUserRepository(LeaderoneDbContext context) : IAppUserRepository
    {
        private readonly LeaderoneDbContext _context = context;

        public async Task AddAsync(AppUser user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            _context.Users.Remove(_context.Users.Find(id) ?? throw new InvalidOperationException("User not found"));
            await _context.SaveChangesAsync();
        }

        public async Task<AppUser?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(user => user.Email == email);
        }

        public async Task<bool> IsExistsAdminAsync(Guid tenantId)
        {
            return await _context.Users.AnyAsync(user => user.TenantId == tenantId && user.RoleInTenant == Domain.Enums.Enumeration.TenantRole.Admin);
        }

        public async Task UpdateAsync(AppUser user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }
}
