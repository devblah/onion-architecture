using System.Linq;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Persistence.Models;

namespace TodoApi.Api.ViewModels
{
    public class TodoListViewModel
    {
        public long Id { get; set; }
        public string Title { get; set; }

        public bool IsArchived { get; set; }

        public List<TodoItemViewModel> Items { get; set; }

        internal static TodoListViewModel FromModel(TodoList model)
        {
            var items = model.Items.Select(TodoItemViewModel.FromModel).ToList();

            return new TodoListViewModel
            {
                Id = model.Id,
                Title = model.Title,
                IsArchived = model.IsArchived,
                Items = items
            };
        }

        internal TodoList ToModel()
        {
            var items = Items.Select(i => i.ToModel()).ToList();
            return new TodoList
            {
                Id = Id,
                Title = Title,
                IsArchived = IsArchived,
                Items = items
            };
        }
    }
}