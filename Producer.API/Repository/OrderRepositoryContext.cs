using Microsoft.EntityFrameworkCore;
using Producer.API.Repository.Entities;

namespace Producer.API.Repository
{
    public class OrderRepositoryContext: DbContext
    {
        public OrderRepositoryContext(DbContextOptions<OrderRepositoryContext> options): base(options) 
        {
            
        }
        public DbSet<Order> order { get; set; }
    }
}
