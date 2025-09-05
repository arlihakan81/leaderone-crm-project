using Leaderone.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leaderone.Domain.Entities
{
    public class Tenant : TEntity<Guid>
    {
        public string Name { get; set; }
        public string Domain { get; set; }

        public List<AppUser> Users { get; set; }
        public List<Customer> Customers { get; set; }
    }
}
