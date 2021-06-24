using System.Threading.Tasks;

namespace TodoApi.Application.Messaging
{
    public interface ITodoListMessagingRepository
    {
         Task<bool> Send(TodoListCreated created);

         Task<bool> Send(ItemAddedToTodoList itemAdded);

         Task<bool> Send(TodoListArchived archived);
    }
}