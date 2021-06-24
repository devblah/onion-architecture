using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TodoApi.Application.Repositories;
using TodoApi.Domain.Repositories;
using TodoApi.Persistence.Models;

namespace TodoApi.Persistence.Repositories
{
    public class TodoRepository : ITodoListPersistenceRepository, ITodoListDomainRepository
    {
        private TodoContext _context;

        public TodoRepository(TodoContext context) {
            _context = context;
        }

        public async Task<Domain.Models.TodoList> GetUnarchivedByTitle(Domain.Models.ItemName name)
        {
            TodoList entity = await _context.TodoList
                .Where(l => l.Title == name.value && !l.IsArchived)
                .Include(l => l.Items)
                .SingleOrDefaultAsync();

            return entity.ToModel();
        }

        public async Task<Domain.Models.TodoList> GetById(Domain.Models.Id<Domain.Models.TodoList> id)
        {
            var entity = await _context.TodoList.Include(l => l.Items).SingleOrDefaultAsync(l => l.Id == id.value);
            
            return entity?.ToModel();
        }

        public async Task<ICollection<Domain.Models.TodoList>> GetAllByTitle(Domain.Models.ItemName name)
        {
            List<TodoList> entities = await _context.TodoList
                .Where(l => l.Title == name.value)
                .Include(l => l.Items)
                .ToListAsync();

            return entities.Select(e => e.ToModel()).ToList();
        }

        public async Task<ICollection<Domain.Models.TodoList>> GetAll()
        {
            List<TodoList> entities = await _context.TodoList
                .Include(l => l.Items)
                .ToListAsync();

            return entities.Select(e => e.ToModel()).ToList();
        }

        public async Task<bool> Save(Domain.Models.TodoList list)
        {
            var existingEntity =  await _context.TodoList.Include(l => l.Items).SingleOrDefaultAsync(l => l.Id == list.Id.value);
            if (existingEntity == null) 
            {
                _context.TodoList.Add(TodoList.FromModel(list));
            }
            else
            {
                Update(existingEntity, list);
            }

            await _context.SaveChangesAsync();

            return true;
        }

        public void Update(TodoList existingEntity, Domain.Models.TodoList list)
        {
            var existingItems = existingEntity.Items.ToList();
            foreach (Domain.Models.TodoItem item in list.Items) 
            {
                var existingItem = existingItems.Where(i => i.Name.ToLower() == item.name.value.ToLower()).SingleOrDefault();

                if (existingItem != null) 
                {
                    existingItem.IsComplete = item.IsCompleted;
                    continue;
                }

                existingEntity.Items.Add(TodoItem.FromModel(item));
            }

            // delete items not present in new collection
            var allExistingNamesLower = new HashSet<string>(list.Items.Select(i => i.name.value.ToLower()));

            // mark old items for deletion
            existingEntity.Items
                .Where(i => !allExistingNamesLower.Contains(i.Name.ToLower()))
                .ToList()
                .ForEach(i => _context.TodoItems.Remove(i));

            // remove from internal items
            existingEntity.Items.RemoveAll(i => !allExistingNamesLower.Contains(i.Name.ToLower()));
            existingEntity.IsArchived = list.IsArchived;
            existingEntity.Title = list.Title.value;
        }

        public async Task<bool> HasUnarchivedListWithTitle(Domain.Models.ItemName title)
        {
            return await _context.TodoList.AnyAsync(l => l.Title == title.value && !l.IsArchived);
        }
    }
}