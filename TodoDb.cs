using Microsoft.EntityFrameworkCore;

/// <summary>
/// TodoDb DbContext class
/// </summary>
public class TodoDb : DbContext
{
    #pragma warning disable CS1591
    public TodoDb(DbContextOptions<TodoDb> options): base(options) { }

    public DbSet<Todo> Todos => Set<Todo>();
    #pragma warning restore CS1591
}
