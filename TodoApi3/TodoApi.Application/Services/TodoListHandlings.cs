using System;
using System.Threading.Tasks;
using TodoApi.Application.Messaging;
using TodoApi.Application.Repositories;
using TodoApi.Domain.Models;
using TodoApi.Domain.Services;

namespace TodoApi.Application.Services
{
    public class TodoListHandlings
    {
        private CreateNewTodoListService _createService;
        private DearchiveTodoListService _dearchiveService;
        private ITodoListPersistenceRepository _persistenceRepo;
        private ITodoListMessagingRepository _messagingRepo;

        public TodoListHandlings(CreateNewTodoListService createService, ITodoListPersistenceRepository persistenceRepo, ITodoListMessagingRepository messagingRepo, DearchiveTodoListService dearchiveService)
        {
            _createService = createService;
            _persistenceRepo = persistenceRepo;
            _messagingRepo = messagingRepo;
            _dearchiveService = dearchiveService;
        }

        public async Task<Id<TodoList>> Create(ItemName name) 
        {
            var list = await _createService.Create(name);

            if (!await _persistenceRepo.Save(list))
                return null;

            await _messagingRepo.Send(new TodoListCreated(list.Id, list.Title));
            return list.Id;
        }

        public async Task<bool> Archive(Id<TodoList> id) 
        {
            var list = await _persistenceRepo.GetById(id);
            // TODO better error handling
            if (list == null)
                return false;

            try {
                list.Archive();
            } 
            catch (InvalidOperationException) 
            {
                return false;
            }

            if (!await _persistenceRepo.Save(list))
                return false;

            // TODO add event
            //await _messagingRepo.Send(new ItemAddedToTodoList(id, name));

            return true;
        }

        public async Task<Id<TodoList>> Dearchive(Id<TodoList> id) 
        {
            var list = await _persistenceRepo.GetById(id);
            // TODO better error handling
            if (list == null)
                return null;

            TodoList unarchivedList;
            try {
                unarchivedList = await _dearchiveService.Dearchive(list);
            } 
            catch (InvalidOperationException) 
            {
                return null;
            }

            if (!await _persistenceRepo.Save(unarchivedList))
                return null;

            // TODO add event
            await _messagingRepo.Send(new TodoListCreated(unarchivedList.Id, unarchivedList.Title));

            return unarchivedList.Id;
        }
    }
}