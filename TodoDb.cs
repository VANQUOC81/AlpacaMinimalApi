using Microsoft.EntityFrameworkCore;

// This class is internal by default
class TodoDb : DbContext
{
    public TodoDb(DbContextOptions<TodoDb> options): base(options) { }

    public DbSet<Todo> Todos => Set<Todo>();
}
