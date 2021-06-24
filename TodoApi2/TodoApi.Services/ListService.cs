using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Persistence.Models;
using TodoApi.Persistence.Repositories;

namespace TodoApi.Services
{
    public class ListService
    {
        private TodoRepository _todoRepository;

        public ListService(TodoRepository todoRepository) {
            _todoRepository = todoRepository;
        }

        public async Task<TodoList> CreateTodoList(string title) 
        {
            var listWithTitleAlreadyExists = (await _todoRepository.FindByTitle(title)).Any();

            // TODO better Error Handling
            if (listWithTitleAlreadyExists)
                throw new InvalidOperationException("List with title already exists");

            var list = new TodoList
            {
                Title = title,
                Items = new List<TodoItem>()
            };

            await _todoRepository.Persist(list);
            return list;
        }

        public async Task<TodoList> ArchiveList(long id) {
            var list = await _todoRepository.GetListById(id);

            list.IsArchived = true;
            list.Items.ForEach(i => i.IsArchived = true);

            await _todoRepository.SaveChanges(list);

            return list;
        }

        public async Task<TodoList> DeArchiveList(long id) {
            var list = await _todoRepository.GetListById(id);

            if (!list.IsArchived)
                return list;

            var unarchivedList = list.createUnarchived(list);
            return await _todoRepository.Persist(unarchivedList);
        }

        public async Task<TodoList> Update(TodoList todoList)
        {
            var existingList = await _todoRepository.GetListById(todoList.Id);

            if (existingList.IsArchived) 
                throw new InvalidOperationException("The given list is already archived");

            return await _todoRepository.Persist(todoList);
        }
    }
}