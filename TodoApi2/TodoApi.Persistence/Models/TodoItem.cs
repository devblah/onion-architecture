namespace TodoApi.Persistence.Models
{
    public class TodoItem
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }

        public bool IsArchived { get; set; }

        public TodoList List { get; set; }
    }
}