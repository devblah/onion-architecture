using System.Collections.Generic;

namespace TodoApi.Models
{
    public class TodoList
    {
        public long Id { get; set; }
        public string Title { get; set; }

        public List<TodoItem> Items { get; set; }
    }
}