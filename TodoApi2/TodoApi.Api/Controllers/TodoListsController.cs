using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Api.ViewModels;
using TodoApi.Persistence;
using TodoApi.Services;

namespace TodoApi.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TodoListsController : ControllerBase
    {
        private readonly TodoContext _context;
        private readonly ListService _listService;

        public TodoListsController(TodoContext context, ListService listService)
        {
            _context = context;
            _listService = listService;
        }

        // GET: api/TodoLists
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoListViewModel>>> GetTodoList()
        {
            var lists = await _context.TodoList.Include(l => l.Items).ToListAsync();
            return Ok(lists.Select(TodoListViewModel.FromModel));
        }

        // GET: api/TodoLists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoListViewModel>> GetTodoList(long id)
        {
            var todoList = await _context.TodoList.Include(l => l.Items).SingleOrDefaultAsync(l => l.Id == id);

            if (todoList == null)
            {
                return NotFound();
            }

            return TodoListViewModel.FromModel(todoList);
        }

        // PUT: api/TodoLists/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoList(long id, TodoListViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return BadRequest();
            }

            var list = await _listService.Update(viewModel.ToModel());

            return Ok(TodoListViewModel.FromModel(list));
        }

        [HttpPut("{id}/archive")]
        public async Task<IActionResult> PutArchiveTodoList(long id, TodoListViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return BadRequest();
            }

            var list = await _listService.ArchiveList(id);

            return Ok(TodoListViewModel.FromModel(list));
        }

        [HttpPut("{id}/dearchive")]
        public async Task<IActionResult> PutDeArchiveTodoList(long id, TodoListViewModel viewModel)
        {
            if (id != viewModel.Id)
            {
                return BadRequest();
            }

            var list = await _listService.DeArchiveList(id);

            return Ok(TodoListViewModel.FromModel(list));
        }


        // POST: api/TodoLists
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TodoListViewModel>> PostTodoList(TodoListViewModel todoList)
        {
            var list = await _listService.CreateTodoList(todoList.Title);

            return CreatedAtAction("GetTodoList", new { id = todoList.Id }, TodoListViewModel.FromModel(list));
        }

        // DELETE: api/TodoLists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoList(long id)
        {
            var todoList = await _context.TodoList.FindAsync(id);
            if (todoList == null)
            {
                return NotFound();
            }

            _context.TodoList.Remove(todoList);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TodoListExists(long id)
        {
            return _context.TodoList.Any(e => e.Id == id);
        }
    }
}
