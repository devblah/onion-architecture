using System.Collections.Generic;
using System.Threading.Tasks;
using TodoApi.Domain.Models;


namespace TodoApi.Application.Repositories
{
    public interface ITodoListPersistenceRepository
    {
         Task<TodoList> GetUnarchivedByTitle(ItemName name);

         Task<TodoList> GetById(Id<TodoList> id);

         Task<ICollection<TodoList>> GetAllByTitle(ItemName name);

         Task<ICollection<TodoList>> GetAll();

         Task<bool> Save(TodoList list);
    }
}