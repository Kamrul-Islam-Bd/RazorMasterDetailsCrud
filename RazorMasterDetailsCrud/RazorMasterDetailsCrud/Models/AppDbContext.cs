using Microsoft.EntityFrameworkCore;

namespace RazorMasterDetailsCrud.Models
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
        {
        }

        public virtual DbSet<Course> Courses { get; set; }

        public virtual DbSet<Module> Modules { get; set; }

        public virtual DbSet<Student> Students { get; set; }
    }
}
