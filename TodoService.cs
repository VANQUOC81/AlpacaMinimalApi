using Microsoft.EntityFrameworkCore;

/// <summary>
/// 
/// </summary>
public class TodoService
{
    #pragma warning disable CS1591
    public static async Task<IResult> GetAllTodos(TodoDb db)
    {
        return TypedResults.Ok(await db.Todos.Select(x => new TodoItemDTO(x)).ToArrayAsync());
    }
    #pragma warning restore CS1591
}
