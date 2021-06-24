using System;
using System.Threading.Tasks;
using TodoApi.Domain.Models;
using TodoApi.Domain.Repositories;

namespace TodoApi.Domain.Services
{
    public class DearchiveTodoListService
    {
        private ITodoListDomainRepository _todoListRepo;

        public DearchiveTodoListService(ITodoListDomainRepository todoListRepo) 
        {
            _todoListRepo = todoListRepo;
        }

        public async Task<TodoList> Dearchive(TodoList archivedList)
        {
            if (!archivedList.IsArchived) {
                throw new InvalidOperationException("Dearchiving an unarchived list is not possible");
            }

            // What if we already have another unarchived list with the given title? We would create a duplicate, which is not allowed
            // So basically we have to check this behavior here -> Important Business Logic can also be applied to repositories!
            var anotherArchivedListWithSameTitleExists = await _todoListRepo.HasUnarchivedListWithTitle(archivedList.Title);
            if (anotherArchivedListWithSameTitleExists)
                throw new InvalidOperationException("There is already another archived list with this title present");

            var unarchivedList = TodoList.Create(archivedList.Title);
            archivedList.Items.ForEach(i => unarchivedList.AddItem(i.name));

            return unarchivedList;
        }
    }
}