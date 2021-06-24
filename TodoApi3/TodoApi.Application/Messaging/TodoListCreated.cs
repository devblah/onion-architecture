using TodoApi.Domain.Models;

namespace TodoApi.Application.Messaging 
{
    public record TodoListCreated(Id<TodoList> id, ItemName title) {}
}