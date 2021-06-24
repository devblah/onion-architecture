using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TodoApi.Persistence.Models;

namespace TodoApi.Persistence.Repositories
{
    public class TodoRepository
    {
        private TodoContext _context;

        public TodoRepository(TodoContext context) {
            _context = context;
        }

        public async Task<List<TodoList>> FindByTitle(string title) {
            return await _context.TodoList
                .Where(l => l.Title == title)
                .Include(l => l.Items)
                .ToListAsync();
        }

        public async Task<TodoList> GetListById(long id) {
            return await _context.TodoList
                .Include(l => l.Items)
                .SingleAsync(l => l.Id == id);
        }

        public async Task<TodoList> Persist(TodoList list) 
        {
            if (list.Id == 0) {
                if (await _context.TodoList.AnyAsync(l => l.Title.ToLower() == list.Title.ToLower() && l.IsArchived == false))
                    throw new InvalidOperationException("Another list with this title already exists");
                _context.TodoList.Add(list);
            } else {
                await Update(list);
            }
            
            await _context.SaveChangesAsync();

            return list;
        }

        public async Task<TodoList> Update(TodoList todoList)
        {
            var existingList = await GetListById(todoList.Id);

            var existingItems = existingList.Items.ToList();
            foreach (TodoItem item in todoList.Items) 
            {
                var existingItem = existingItems.Where(i => i.Name.ToLower() == item.Name.ToLower()).SingleOrDefault();

                if (existingItem != null) 
                {
                    existingItem.IsComplete = item.IsComplete;
                    continue;
                }

                item.List = existingList;
                existingList.Items.Add(item);
            }

            // delete items not present in new collection
            var allExistingNamesLower = new HashSet<string>(todoList.Items.Select(i => i.Name.ToLower()));

            // mark old items for deletion
            existingList.Items
                .Where(i => !allExistingNamesLower.Contains(i.Name.ToLower()))
                .ToList()
                .ForEach(i => _context.TodoItems.Remove(i));

            // remove from internal items
            existingList.Items.RemoveAll(i => !allExistingNamesLower.Contains(i.Name.ToLower()));

            return existingList;
        }

        public async Task SaveChanges(TodoList list)
        {
            await _context.SaveChangesAsync();
        }
    }
}