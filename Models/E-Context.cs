using Microsoft.EntityFrameworkCore;

namespace E_Commerce.Models
{
    public class E_Context : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public E_Context(DbContextOptions options) : base(options) { }
        public DbSet<User> Users { get; set; }
    }
}