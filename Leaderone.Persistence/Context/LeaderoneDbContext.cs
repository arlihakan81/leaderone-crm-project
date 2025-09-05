using Leaderone.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Leaderone.Persistence.Context
{
    public class LeaderoneDbContext : DbContext
    {
        public DbSet<Tenant> Tenants { get; set; }
        public DbSet<AppUser> Users { get; set; }
        public DbSet<Customer> Customers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Server=(localdb)\mssqllocaldb; Database=LeaderoneDb; Trusted_Connection=True; TrustServerCertificate=True");
        }


    }
}
