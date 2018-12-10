using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoController : ControllerBase
    {
        private readonly TodoContext _context;
        public TodoController(TodoContext context)
        {
            _context = context;

            if (_context.TodoItems.Count() == 0)
            {
                // Create an item if collection is empty
                _context.TodoItems.Add(new TodoItem { Name = "TodoItem1" });
                _context.SaveChanges();
            }
        }

        // GET: api/Todo
        [HttpGet]
        public async Task<ActionResult<IOrderedEnumerable<TodoItem>>> GetTodoItems()
        {
            var todoItems = await _context.TodoItems.ToListAsync();
            return Ok(todoItems);
        }

        // Get: api/Todo/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItem>> GetTodoItem(int id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }
            return todoItem;
        }

        // POST: api/Todo
        [HttpPost]
        public async Task<ActionResult<TodoItem>> PostTodoItem(TodoItem todoitem)
        {
            _context.TodoItems.Add(todoitem);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetTodoItem", new TodoItem { Id = todoitem.Id }, todoitem);
        }

        // PUT: api/Todo/5
        [HttpPut("{id}")]
        public async Task<ActionResult<TodoItem>> PutTodoItem(int id, TodoItem todoitem)
        {
            if (id != todoitem.Id)
            {
                return BadRequest();
            }
            _context.Entry(todoitem).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/Todo/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<TodoItem>> DeleteTodoItem(int id)
        {
            var todoItem = await _context.TodoItems.FindAsync(id);
            if (todoItem == null)
            {
                return NotFound();
            }
            _context.TodoItems.Remove(todoItem);
            await _context.SaveChangesAsync();
            return todoItem;
        }
    }
}