using System.Runtime.InteropServices.ComTypes;
using System.Linq;
using System;
using System.Collections.Generic;

namespace TodoApi.Persistence.Models
{
    public class TodoList
    {
        public long Id { get; set; }
        public string Title { get; set; }

        public bool IsArchived { get; set; }

        public List<TodoItem> Items { get; set; }

        public TodoList createUnarchived(TodoList list)
        {
            var newList = new TodoList
            {
                Title = Title,
                IsArchived = false
            };

            var newItems = Items.Select(i => new TodoItem() 
            {
                Name = i.Name,
                IsComplete = i.IsComplete,
                List = list
            }).ToList();

            newList.Items = newItems;

            return newList;
        }
    }
}