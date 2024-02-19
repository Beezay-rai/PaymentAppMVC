using Microsoft.EntityFrameworkCore;

namespace PayementMVC.Data
{
    public class PaymentDbContext : DbContext
    {
        public PaymentDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Balance> Balance { get; set; }
        public DbSet<Transaction> Transaction { get; set; }

    }
}
