using Leaderone.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leaderone.Application.Interfaces
{
    public interface IAppUserRepository
    {
        Task<AppUser?> GetUserByEmailAsync(string email);
        Task<bool> IsExistsAdminAsync(Guid tenantId);
        Task AddAsync(AppUser user);
        Task UpdateAsync(AppUser user);
        Task DeleteAsync(Guid id);
    }
}
