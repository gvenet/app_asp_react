using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Models;

namespace TodoApi.Controllers {
  [Route("api/[controller]")]
  [ApiController]
  public class ProductController : ControllerBase {
    private readonly ApiContext _context;

    public ProductController(ApiContext context) {
      _context = context;
    }

    // GET: api/Product
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProduct(
        [FromQuery] string? category,
        [FromQuery] float minPrice,
        [FromQuery] float maxPrice,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10
    ) {
      var query = _context.Products.Include(p => p.Category).AsQueryable();

      if (!string.IsNullOrEmpty(category)) {
        query = query.Where(p => p.Category.Label == category);
      }

      if (minPrice > 0) {
        query = query.Where(p => p.Price >= minPrice);
      }

      if (maxPrice > 0) {
        query = query.Where(p => p.Price <= maxPrice);
      }

      int totalItems = await query.CountAsync();
      int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

      if (page < 1 || page > totalPages) {
        return BadRequest("Page invalide");
      }

      int startIndex = (page - 1) * pageSize;
      query = query.Skip(startIndex).Take(pageSize);

      Response.Headers.Add("X-Total-Pages", totalPages.ToString());
      
      
      
      var products = await query.ToListAsync();

      if (products == null) {
        return NotFound();
      }


      return products;
    }


    // GET: api/Product/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(long id) {
      if (_context.Products == null) {
        return NotFound();
      }
      var product = await _context.Products.FindAsync(id);

      if (product == null) {
        return NotFound();
      }

      return product;
    }

    // PUT: api/Product/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutProduct(long id, Product product) {
      if (id != product.Id) {
        return BadRequest();
      }

      _context.Entry(product).State = EntityState.Modified;

      try {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException) {
        if (!ProductExists(id)) {
          return NotFound();
        }
        else {
          throw;
        }
      }

      return NoContent();
    }

    // POST: api/Product
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Product>> PostProduct(Product product) {
      if (_context.Products == null) {
        return Problem("Entity set 'Context.Product'  is null.");
      }
      _context.Products.Add(product);
      await _context.SaveChangesAsync();

      return CreatedAtAction("GetProduct", new { id = product.Id }, product);
    }

    // DELETE: api/Product/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(long id) {
      if (_context.Products == null) {
        return NotFound();
      }
      var product = await _context.Products.FindAsync(id);
      if (product == null) {
        return NotFound();
      }

      _context.Products.Remove(product);
      await _context.SaveChangesAsync();

      return NoContent();
    }

    private bool ProductExists(long id) {
      return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
    }
  }
}
