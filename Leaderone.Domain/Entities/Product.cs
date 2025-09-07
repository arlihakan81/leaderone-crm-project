using Leaderone.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leaderone.Domain.Entities
{
    public class Product : TEntity<Guid>
    {
        public string? ImageUrl { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public Guid CategoryId { get; set; }
        public Guid VendorId { get; set; }
        public Guid TenantId { get; set; }

        public Category Category { get; set; }
        public Vendor Vendor { get; set; }
        public Tenant Tenant { get; set; }

    }
}
