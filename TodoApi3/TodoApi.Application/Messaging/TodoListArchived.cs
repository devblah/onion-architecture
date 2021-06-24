using TodoApi.Domain.Models;

namespace TodoApi.Application.Messaging
{
    public record TodoListArchived(Id<TodoList> id, ItemName title)
    {
        
    }
}