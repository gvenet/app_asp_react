using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Controllers {
  [Route("api/[controller]")]
  [ApiController]
  public class CategoryController : ControllerBase {
    private readonly ApiContext _context;

    public CategoryController(ApiContext context) {
      _context = context;
    }

    // GET: api/Category
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Category>>> GetCategories() {
      var categories = await _context.Categories
          .Include(category => category.Products) // Inclure les produits associés
          .ToListAsync();

      if (categories == null) {
        return NotFound();
      }

      return categories;
    }

    // GET: api/Category/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Category>> GetCategory(long id) {
      if (_context.Categories == null) {
        return NotFound();
      }
      var category = await _context.Categories.FindAsync(id);

      if (category == null) {
        return NotFound();
      }

      return category;
    }

    // PUT: api/Category/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutCategory(long id, Category category) {
      if (id != category.Id) {
        return BadRequest();
      }

      _context.Entry(category).State = EntityState.Modified;

      try {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException) {
        if (!CategoryExists(id)) {
          return NotFound();
        }
        else {
          throw;
        }
      }

      return NoContent();
    }

    // POST: api/Category
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Category>> PostCategory(Category category) {
      if (_context.Categories == null) {
        return Problem("Entity set 'ApiContext.Categories'  is null.");
      }
      _context.Categories.Add(category);
      await _context.SaveChangesAsync();

      return CreatedAtAction("GetCategory", new { id = category.Id }, category);
    }

    // DELETE: api/Category/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCategory(long id) {
      if (_context.Categories == null) {
        return NotFound();
      }
      var category = await _context.Categories.FindAsync(id);
      if (category == null) {
        return NotFound();
      }

      _context.Categories.Remove(category);
      await _context.SaveChangesAsync();

      return NoContent();
    }

    private bool CategoryExists(long id) {
      return (_context.Categories?.Any(e => e.Id == id)).GetValueOrDefault();
    }
  }
}
