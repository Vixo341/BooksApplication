using BooksApplication.Models;
using Microsoft.EntityFrameworkCore;

namespace BooksApplication.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Category> Categories { get; set; }
    }
}
