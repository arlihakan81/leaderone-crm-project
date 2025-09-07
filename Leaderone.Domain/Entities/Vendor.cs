using Leaderone.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leaderone.Domain.Entities
{
    public class Vendor : TEntity<Guid>
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string? Phone { get; set; }

        public Guid TenantId { get; set; }
        public Tenant Tenant { get; set; }


    }
}
