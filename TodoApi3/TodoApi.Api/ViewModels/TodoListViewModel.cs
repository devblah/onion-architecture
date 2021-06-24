using System.Linq;
using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using TodoApi.Domain.Models;

namespace TodoApi.Api.ViewModels
{
    public class TodoListViewModel
    {
        public Guid Id { get; set; }
        public string Title { get; set; }

        public bool IsArchived { get; set; }

        public List<TodoItemViewModel> Items { get; set; }

        internal static TodoListViewModel FromModel(TodoList model)
        {
            var items = model.Items.Select(TodoItemViewModel.FromModel).ToList();

            return new TodoListViewModel
            {
                Id = model.Id.ToGuid(),
                Title = model.Title.value,
                IsArchived = model.IsArchived,
                Items = items
            };
        }
    }
}