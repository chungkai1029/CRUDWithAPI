using Microsoft.EntityFrameworkCore;
using System.Diagnostics.CodeAnalysis;

namespace CRUDWithAPI.Models
{
    public class CustomerContext : DbContext
    {
        public CustomerContext(DbContextOptions<CustomerContext> options)
            : base(options)
        {
        }

        public DbSet<Customer> Customers { get; set; } = null!;
    }
}