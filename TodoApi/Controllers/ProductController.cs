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

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Product>>> GetProduct(
        [FromQuery] string? category,
        [FromQuery] string? brand,
        [FromQuery] float minPrice,
        [FromQuery] float maxPrice,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10
    ) {
      if (page == 0) {
        page = 1;
      }

      float maxProductPrice = await _context.Products.MaxAsync(p => p.Price);

      var query = _context.Products.AsQueryable();

      if (!string.IsNullOrEmpty(brand)) {
        query = query.Where(b => b.Brand!.Label == brand);
      }

      if (!string.IsNullOrEmpty(category)) {
        query = query
            .Where(p => p.ProductCategories.Any(pc => pc.Category!.Label == category));
      }

      if (minPrice > maxPrice) {
        return Ok(new List<object>());
      }

      if (minPrice > 0) {
        query = query.Where(p => p.Price >= minPrice);
      }

      if (maxPrice > 0) {
        query = query.Where(p => p.Price <= maxPrice);
      }

      int totalItems = await query.CountAsync();

      if (totalItems == 0) {
        return Ok(new List<object>());
      }

      int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

      if (page > totalPages) {
        page = totalPages;
      }
      
      if (page < 1) {
        return Ok(new List<object>());
      }

      int startIndex = (page - 1) * pageSize;

      var products = await query
          .Skip(startIndex)
          .Take(pageSize)
          .Include(p => p.ProductCategories)
          .ThenInclude(pc => pc.Category)
          .Include(p => p.Brand)
          .ToListAsync();

      if (products == null || products.Count == 0) {
        return Ok(new List<object>());
      }

      Response.Headers.Add("X-Total-Pages", totalPages.ToString());

      Response.Headers.Add("X-Max-Price", maxProductPrice.ToString());

      Response.Headers.Add("X-Current-Page", page.ToString());

      var productsWithCategories = products.Select(p => new {
        p.Id,
        p.Label,
        p.Price,
        p.Description,
        p.Image_Url,
        p.Version,
        p.Brand,
        Categories = p.ProductCategories.Select(pc => pc.Category!.Label).ToList()
      });

      return Ok(productsWithCategories);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Product>> GetProduct(long id) {
      var product = await _context.Products
          .Include(p => p.ProductCategories)
          .ThenInclude(pc => pc.Category)
          .FirstOrDefaultAsync(p => p.Id == id);

      if (product == null) {
        return NotFound();
      }

      var productWithCategories = new {
        product.Id,
        product.Label,
        product.Price,
        product.Description,
        product.Image_Url,
        product.Version,
        Categories = product.ProductCategories.Select(pc => pc.Category!.Label).ToList()
      };

      return Ok(productWithCategories);
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
