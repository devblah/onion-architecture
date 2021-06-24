using System.Linq;
using System;
using System.Collections.Generic;

namespace TodoApi.Domain.Models
{
    public class TodoList
    {
        public Id<TodoList> Id { get; private set; }

        public ItemName Title { get; private set; }

        public bool IsArchived { get; private set; }

        public List<TodoItem> Items { get; private set; }

        // This method is the only "infrastructrure" code here. You can use this to generate your model from external source
        public static TodoList Hydrate(
            string id,
            string title,
            bool isArchived,
            List<TodoItem> items
        ) {
            return new TodoList {
                Id = Id<TodoList>.FromString(id),
                Title = new ItemName(title),
                IsArchived = isArchived,
                Items = items
            };
        }

        public static TodoList Create(ItemName title) 
        {
            return new TodoList {
                Id = new Id<TodoList>(),
                Title = title,
                IsArchived = false,
                Items = new List<TodoItem>()
            };
        }

        public void Archive() {
            IsArchived = true;
        }

        public void Rename(ItemName newTitle) 
        {
            if (IsArchived)
                throw new InvalidOperationException("you can not rename an archived list");

            Title = newTitle;
        }

        public void AddItem(ItemName itemName) {
            if (IsArchived)
                throw new InvalidOperationException("you can not add an item to an archived list");

            if (_ItemWithNameDoesExist(itemName))
                throw new InvalidOperationException("there is already a todo item with the same name");
            
            Items.Add(new TodoItem(itemName, false));
        }

        public void CheckItem(ItemName itemName) {
            if (IsArchived)
                throw new InvalidOperationException("you can not check an item of an archived list");

            if (!_ItemWithNameDoesExist(itemName)) 
                throw new InvalidOperationException($"There is no item with the name {itemName}");
            
            Items.RemoveAll(i => i.name == itemName);
            Items.Add(new TodoItem(itemName, true));
        }

        public void UncheckItem(ItemName itemName) {
            if (IsArchived)
                throw new InvalidOperationException("you can not uncheck an item of an archived list");

            if (!_ItemWithNameDoesExist(itemName)) 
                throw new InvalidOperationException($"There is no item with the name {itemName}");
            
            Items.RemoveAll(i => i.name == itemName);
            Items.Add(new TodoItem(itemName, false));
        }

        public void RenameItem(ItemName oldName, ItemName newName) {
            if (IsArchived)
                throw new InvalidOperationException("you can not rename an item of an archived list");

            if (!_ItemWithNameDoesExist(oldName))
                throw new InvalidOperationException($"There is no item with the name {oldName}");

            var oldItem = Items.Single(i => i.name == oldName);
            if (oldItem.IsCompleted)
                throw new InvalidOperationException($"Completed Items are not allowed to be renamed.");

            Items.Remove(oldItem);
            Items.Add(new TodoItem(newName, false));
        }

        private bool _ItemWithNameDoesExist(ItemName name) 
        {
            return Items.Any(i => i.name == name);
        }
    }
}