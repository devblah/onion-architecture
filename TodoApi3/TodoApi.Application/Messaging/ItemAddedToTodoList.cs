using TodoApi.Domain.Models;

namespace TodoApi.Application.Messaging
{
    public record ItemAddedToTodoList(Id<TodoList> id, ItemName name) {
        
    }
}