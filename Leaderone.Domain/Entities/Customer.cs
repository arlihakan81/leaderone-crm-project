using Leaderone.Domain.Common;
using Leaderone.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leaderone.Domain.Entities
{
    public class Customer : TEntity<Guid>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Address { get; set; }
        public Enumeration.CustomerStatus Status { get; set; }
        public Guid TenantId { get; set; }
        public Tenant Tenant { get; set; }

    }
}
