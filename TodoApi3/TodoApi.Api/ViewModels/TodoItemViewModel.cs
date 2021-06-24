using System;
using TodoApi.Domain.Models;

namespace TodoApi.Api.ViewModels
{
    public class TodoItemViewModel
    {
        public string Name { get; set; }
        public bool IsComplete { get; set; }

        internal static TodoItemViewModel FromModel(TodoItem model)
        {
            return new TodoItemViewModel
            {
                Name = model.name.value,
                IsComplete = model.IsCompleted
            };
        }
    }
}