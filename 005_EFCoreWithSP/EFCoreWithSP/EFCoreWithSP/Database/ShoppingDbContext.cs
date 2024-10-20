using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EFCoreWithSP.Database
{
    internal class ShoppingDbContext : DbContext
    {
        public DbSet<OrderDetail> OrderDetails { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=ShoppingDb;Trusted_Connection=True;TrustServerCertificate=True");
        }
    }
}
