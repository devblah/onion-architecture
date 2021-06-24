using System;

namespace TodoApi.Persistence.Models
{
    public class TodoItem
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }

        public bool IsArchived { get; set; }

        public TodoList List { get; set; }

        internal Domain.Models.TodoItem ToModel()
        {
            return new Domain.Models.TodoItem(new Domain.Models.ItemName(Name), IsComplete);
        }

        internal static TodoItem FromModel(Domain.Models.TodoItem i)
        {
            return new TodoItem() {
                Id = new Guid(),
                Name = i.name.value,
                IsComplete = i.IsCompleted
            };
        }
    }
}