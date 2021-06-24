using System.Threading.Tasks;
using TodoApi.Application.Messaging;

namespace TodoApi.Persistence.Repositories
{
    public class MockMessagingRepository : ITodoListMessagingRepository
    {
        public Task<bool> Send(TodoListCreated created)
        {
            return Task.FromResult(true);
        }

        public Task<bool> Send(ItemAddedToTodoList itemAdded)
        {
            return Task.FromResult(true);
        }

        public Task<bool> Send(TodoListArchived archived)
        {
            return Task.FromResult(true);
        }
    }
}