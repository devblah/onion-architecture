using Microsoft.EntityFrameworkCore;

using TodoApi.Persistence.Models;

namespace TodoApi.Persistence
{
    public class TodoContext : DbContext
    {
        public TodoContext(DbContextOptions<TodoContext> options)
            : base(options)
        {
            
        }

        public DbSet<TodoItem> TodoItems { get; set; }

        public DbSet<TodoList> TodoList { get; set; }
    }
}