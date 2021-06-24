using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Api.ViewModels;
using TodoApi.Application.Repositories;
using TodoApi.Application.Services;
using TodoApi.Domain.Models;

namespace TodoApi.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TodoListsController : ControllerBase
    {
        private readonly ITodoListPersistenceRepository _persistenceRepo;
        private readonly TodoListHandlings _todoListHandlings;
        private readonly TodoListItemHandlings _itemHandlings;


        public TodoListsController(
            ITodoListPersistenceRepository persistenceRepo,
            TodoListHandlings todoListHandlings, TodoListItemHandlings itemHandlings)
        {
            _persistenceRepo = persistenceRepo;
            _todoListHandlings = todoListHandlings;
            _itemHandlings = itemHandlings;
        }

        // GET: api/TodoLists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoListViewModel>>> GetTodoList()
        {
            var lists = await _persistenceRepo.GetAll();
            return Ok(lists.Select(TodoListViewModel.FromModel));
        }

        // GET: api/TodoLists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoListViewModel>> GetTodoList(Guid id)
        {
            var todoList = await _persistenceRepo.GetById(new Id<TodoList>(id));

            if (todoList == null)
            {
                return NotFound();
            }

            return TodoListViewModel.FromModel(todoList);
        }

        // PUT: api/TodoLists/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}/add-item")]
        public async Task<IActionResult> AddItem(Guid id, [FromBody] string name)
        {
            var typedId = new Id<TodoList>(id);
            var result = await _itemHandlings.AddItem(typedId, new ItemName(name));
            
            if (!result)
                return BadRequest();

            var list = await _persistenceRepo.GetById(typedId);

            return Ok(TodoListViewModel.FromModel(list));
        }

        [HttpPut("{id}/check-item")]
        public async Task<IActionResult> CheckItem(Guid id, [FromBody] string name)
        {
            var typedId = new Id<TodoList>(id);
            var result = await _itemHandlings.CheckItem(typedId, new ItemName(name));
            
            if (!result)
                return BadRequest();

            var list = await _persistenceRepo.GetById(typedId);

            return Ok(TodoListViewModel.FromModel(list));
        }

        [HttpPut("{id}/uncheck-item")]
        public async Task<IActionResult> UncheckItem(Guid id, [FromBody] string name)
        {
            var typedId = new Id<TodoList>(id);
            var result = await _itemHandlings.UncheckItem(typedId, new ItemName(name));
            
            if (!result)
                return BadRequest();

            var list = await _persistenceRepo.GetById(typedId);

            return Ok(TodoListViewModel.FromModel(list));
        }

        [HttpPut("{id}/archive")]
        public async Task<IActionResult> ArchiveTodoList(Guid id)
        {
            var typedId = new Id<TodoList>(id);
            var result = await _todoListHandlings.Archive(typedId);
            
            if (!result)
                return BadRequest();

            var list = await _persistenceRepo.GetById(typedId);

            return Ok(TodoListViewModel.FromModel(list));
        }

        [HttpPut("{id}/dearchive")]
        public async Task<IActionResult> DearchiveTodoList(Guid id)
        {
            var typedId = new Id<TodoList>(id);
            var newId = await _todoListHandlings.Dearchive(typedId);
            
            if (newId == null)
                return BadRequest();

            var list = await _persistenceRepo.GetById(newId);

            return CreatedAtAction("GetTodoList", new { id = list.Id.ToGuid() }, TodoListViewModel.FromModel(list));
        }


        // POST: api/TodoLists
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TodoListViewModel>> PostTodoList([FromBody] string title)
        {
            var id = await _todoListHandlings.Create(new ItemName(title));
            if (id == null)
                return BadRequest();

            var list = await _persistenceRepo.GetById(id);

            return CreatedAtAction("GetTodoList", new { id = list.Id.ToGuid() }, TodoListViewModel.FromModel(list));
        }
    }
}
