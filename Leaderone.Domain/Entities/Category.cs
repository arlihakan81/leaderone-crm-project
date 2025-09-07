using Leaderone.Domain.Common;

namespace Leaderone.Domain.Entities
{
    public class Category : TEntity<Guid>
    {
        public string Name { get; set; }
        public string? Description { get; set; } 

        public Guid TenantId { get; set; }
        public Tenant Tenant { get; set; }
    }
}