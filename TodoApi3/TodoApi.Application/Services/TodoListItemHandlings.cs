using System;
using System.Threading.Tasks;
using TodoApi.Application.Messaging;
using TodoApi.Application.Repositories;
using TodoApi.Domain.Models;

namespace TodoApi.Application.Services
{
    public class TodoListItemHandlings
    {
        private ITodoListPersistenceRepository _persistenceRepo;

        private ITodoListMessagingRepository _messagingRepo;

        public TodoListItemHandlings(ITodoListPersistenceRepository persistenceRepo, ITodoListMessagingRepository messagingRepo)
        {
            _persistenceRepo = persistenceRepo;
            _messagingRepo = messagingRepo;
        }

        public async Task<bool> AddItem(Id<TodoList> id, ItemName name)
        {
            var list = await _persistenceRepo.GetById(id);
            
            // TODO better error handling
            if (list == null)
                return false;

            try {
                list.AddItem(name);
            } 
            catch (InvalidOperationException)
            {
                return false;
            }

            if (!await _persistenceRepo.Save(list))
                return false;

            await _messagingRepo.Send(new ItemAddedToTodoList(id, name));

            return true;
        }

        public async Task<bool> RenameItem(Id<TodoList> id, ItemName oldName, ItemName newName) 
        {
            var list = await _persistenceRepo.GetById(id);
            
            // TODO better error handling
            if (list == null)
                return false;

            try {
                list.RenameItem(oldName, newName);
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

        public async Task<bool> CheckItem(Id<TodoList> id, ItemName itemName) 
        {
            var list = await _persistenceRepo.GetById(id);
            
            // TODO better error handling
            if (list == null)
                return false;

            try {
                list.CheckItem(itemName);
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

        public async Task<bool> UncheckItem(Id<TodoList> id, ItemName itemName) 
        {
            var list = await _persistenceRepo.GetById(id);
            
            // TODO better error handling
            if (list == null)
                return false;

            try {
                list.UncheckItem(itemName);
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
    }
}