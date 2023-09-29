using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Controllers {
  [Route("api/[controller]")]
  [ApiController]
  public class TodoController : ControllerBase {
    private readonly ApiContext _context;

    public TodoController(ApiContext context) {
      _context = context;
    }

    // GET: api/Todo
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Todo>>> GetTodo() {
      if (_context.Todos == null) {
        return NotFound();
      }
      return await _context.Todos.ToListAsync();
    }

    // GET: api/Todo/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Todo>> GetTodo(long id) {
      if (_context.Todos == null) {
        return NotFound();
      }
      var Todo = await _context.Todos.FindAsync(id);

      if (Todo == null) {
        return NotFound();
      }
      return Todo;
    }

    // PUT: api/Todo/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutTodo(long id, Todo Todo) {
      if (id != Todo.Id) {
        return BadRequest();
      }

      _context.Entry(Todo).State = EntityState.Modified;
      try {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException) {
        if (!TodoExists(id)) {
          return NotFound();
        }
        else {
          throw;
        }
      }

      return NoContent();
    }

    // POST: api/Todo
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Todo>> PostTodo(Todo Todo) {
      if (_context.Todos == null) {
        return Problem("Entity set 'ApiContext.Todo'  is null.");
      }
      _context.Todos.Add(Todo);
      await _context.SaveChangesAsync();

      // return CreatedAtAction("GetTodo", new { id = Todo.Id }, Todo);
      return CreatedAtAction(nameof(GetTodo), new { id = Todo.Id }, Todo);
    }

    // DELETE: api/Todo/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodo(long id) {
      if (_context.Todos == null) {
        return NotFound();
      }
      var Todo = await _context.Todos.FindAsync(id);
      if (Todo == null) {
        return NotFound();
      }

      _context.Todos.Remove(Todo);
      await _context.SaveChangesAsync();

      return NoContent();
    }

    private bool TodoExists(long id) {
      return (_context.Todos?.Any(e => e.Id == id)).GetValueOrDefault();
    }
  }
}
