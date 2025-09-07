using Leaderone.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leaderone.Application.DTOs
{
    public class CategoryDTO : TEntity<Guid>
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public Guid TenantId { get; set; }

    }
}
