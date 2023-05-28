using Microsoft.EntityFrameworkCore;
using OPizza.Models;

namespace OPizza.Context
{
    public class PizzaDbContext : DbContext
    {
        public DbSet<Pizza> Pizzas { get; set; }

        public PizzaDbContext(DbContextOptions<PizzaDbContext> options) : base(options)
        {

        }
    }
}
