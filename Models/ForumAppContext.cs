using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ForumApp.Models
{
    public class ForumAppContext : IdentityDbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        
        
        public ForumAppContext(DbContextOptions options) : base(options)
        {
            
        }
    }
}