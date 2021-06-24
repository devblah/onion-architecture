using System.Runtime.InteropServices.ComTypes;
using System.Linq;
using System;
using System.Collections.Generic;
using TodoApi.Domain.Models;

namespace TodoApi.Persistence.Models
{
    public class TodoList
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public bool IsArchived { get; set; }

        public List<TodoItem> Items { get; set; }

        internal Domain.Models.TodoList ToModel()
        {
            return Domain.Models.TodoList.Hydrate(
                Id.ToString(),
                Title,
                IsArchived,
                Items.Select(i => i.ToModel()).ToList()                
            );
        }

        internal static TodoList FromModel(Domain.Models.TodoList list)
        {
            var items = list.Items.Select(i => TodoItem.FromModel(i)).ToList();

            return new TodoList() 
            {
                Id = list.Id.ToGuid(),
                Title = list.Title.value,
                IsArchived = list.IsArchived,
                Items = items
            };
        }
    }
}