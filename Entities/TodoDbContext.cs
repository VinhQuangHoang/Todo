using Microsoft.EntityFrameworkCore;

namespace Todolist.Entities
{
    public class TodoDbContext : DbContext
    {
        public TodoDbContext(DbContextOptions<TodoDbContext> options) : base(options)
        {
        }
        public DbSet<Todo> Todos { get; set; }
    }
}
