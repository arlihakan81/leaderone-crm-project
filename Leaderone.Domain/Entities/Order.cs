using Leaderone.Domain.Common;
using Leaderone.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leaderone.Domain.Entities
{
    public class Order : TEntity<Guid>
    {
        public Guid CustomerId { get; set; }
        public Enumeration.OrderStatus Status { get; set; }
        public Enumeration.PaymentStatus PaymentStatus { get; set; }
        public Enumeration.DeliveryType DeliveryType { get; set; }

        public DateTime Date { get; set; }

    }
}
