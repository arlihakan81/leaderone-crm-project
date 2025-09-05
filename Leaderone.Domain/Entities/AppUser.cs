using Leaderone.Domain.Common;
using Leaderone.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leaderone.Domain.Entities
{
    public class AppUser : TEntity<Guid>
    {
        public string? Avatar { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName => $"{FirstName} {LastName}";
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public Guid TenantId { get; set; }
        public Tenant Tenant { get; set; }
        public Enumeration.TenantRole RoleInTenant { get; set; }
    }
}
