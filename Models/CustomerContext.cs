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

        #if !WINDOWS
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //  warning To protect potentially sensitive information in your connection string, 
            //  you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 
            //  for guidance on storing connection strings.
             
           optionsBuilder.UseMySQL("server=localhost;database=Northwind;user=root;password=20190928");
        }
        #endif
    }
}