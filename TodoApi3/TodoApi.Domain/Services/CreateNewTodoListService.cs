using System;
using System.Threading.Tasks;
using TodoApi.Domain.Models;
using TodoApi.Domain.Repositories;

namespace TodoApi.Domain.Services
{
    public class CreateNewTodoListService
    {
        private ITodoListDomainRepository _todoListRepository;

        public CreateNewTodoListService(ITodoListDomainRepository todoListRepository)
        {
            _todoListRepository = todoListRepository;
        }

        public async Task<TodoList> Create(ItemName title) {
            var unarchivedListWithGivenTitleAlreadyExists = await _todoListRepository.HasUnarchivedListWithTitle(title);
            if (unarchivedListWithGivenTitleAlreadyExists)
                throw new InvalidOperationException("An unarchived list with this title already exists.");

            return TodoList.Create(title);
        }
    }
}