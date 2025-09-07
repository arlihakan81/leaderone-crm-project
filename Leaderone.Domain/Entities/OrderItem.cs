using Leaderone.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Leaderone.Domain.Entities
{
    public class OrderItem : TEntity<Guid>
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }

        public decimal Total => Product.Price * Quantity;

        public Order Order { get; set; }
        public Product Product { get; set; }

    }
}
