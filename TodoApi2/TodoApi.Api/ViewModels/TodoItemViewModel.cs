using System;
using TodoApi.Persistence.Models;

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
                Name = model.Name,
                IsComplete = model.IsComplete
            };
        }

        internal TodoItem ToModel()
        {
            return new TodoItem
            {
                Name = Name,
                IsComplete = IsComplete
            };
        }
    }
}