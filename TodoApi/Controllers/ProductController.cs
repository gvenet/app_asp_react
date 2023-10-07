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
        [FromQuery] string? sortColumn,
        [FromQuery] string? sortOrder,
        [FromQuery] string? categories,
        [FromQuery] string? brand,
        [FromQuery] float minPrice,
        [FromQuery] float maxPrice,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10
    ) {

      // Récupération du prix maximal parmi tous les produits
      float maxProductPrice = await _context.Products.MaxAsync(p => p.Price);

      // Création d'une requête IQueryable pour les produits
      var query = _context.Products.AsQueryable();

      // Filtrage par marque si une marque est spécifiée
      if (!string.IsNullOrEmpty(brand)) {
        query = query.Where(b =>
            b.Brand != null &&
            b.Brand.Label == brand);
      }

      // Filtrage par catégories si des catégories sont spécifiées
      if (categories != null) {
        string[] categoriesArr = categories.Split(',');

        if (categoriesArr.Length > 0) {
          // Création d'une liste de catégories à filtrer
          var categoriesToMatch = categoriesArr.Where(cat => !string.IsNullOrEmpty(cat)).ToList();

          // Filtrage des produits pour s'assurer qu'ils ont toutes les catégories spécifiées
          foreach (var cat in categoriesToMatch) {
            query = query.Where(p => p.ProductCategories
                .Any(pc => pc.Category != null && pc.Category.Label != null && pc.Category.Label == cat));
          }
        }
      }

      // Validation du prix minimum et maximum
      if (minPrice > maxPrice) {
        return NotFound();
      }

      // Filtrage par prix minimum si un prix minimum est spécifié
      if (minPrice > 0) {
        query = query.Where(p => p.Price >= minPrice);
      }

      // Filtrage par prix maximum si un prix maximum est spécifié
      if (maxPrice > 0) {
        query = query.Where(p => p.Price <= maxPrice);
      }

      if (!string.IsNullOrEmpty(sortColumn)) {
        switch (sortColumn.ToLower()) {
          case "id":
            query = sortOrder == "desc" ? query.OrderByDescending(p => p.Id) : query.OrderBy(p => p.Id);
            break;
          case "label":
            query = sortOrder == "desc" ? query.OrderByDescending(p => p.Label) : query.OrderBy(p => p.Label);
            break;
          case "price":
            query = sortOrder == "desc" ? query.OrderByDescending(p => p.Price) : query.OrderBy(p => p.Price);
            break;
          case "description":
            query = sortOrder == "desc" ? query.OrderByDescending(p => p.Description) : query.OrderBy(p => p.Description);
            break;
          case "image_url":
            query = sortOrder == "desc" ? query.OrderByDescending(p => p.Image_Url) : query.OrderBy(p => p.Image_Url);
            break;
          case "version":
            query = sortOrder == "desc" ? query.OrderByDescending(p => p.Version) : query.OrderBy(p => p.Version);
            break;
          case "brand.label":
            query = sortOrder == "desc" ? query.OrderByDescending(p => p.Brand.Label) : query.OrderBy(p => p.Brand.Label);
            break;
          default:
            // Gestion du tri par défaut en cas de colonne inconnue
            query = query.OrderBy(p => p.Id);
            break;
        }
      }

      // Calcul du nombre total d'éléments dans la requête
      int totalItems = await query.CountAsync();

      // Si aucun élément ne correspond aux critères, renvoie NotFound
      if (totalItems == 0) {
        return NotFound();
      }

      // Calcul du nombre total de pages en fonction de la taille de la page
      int totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

      // Correction de la page si elle est supérieure au nombre total de pages
      if (page > totalPages) {
        page = totalPages;
      }

      // Correction de la page si elle est inférieure à 1
      if (page < 1) {
        return NotFound();
      }

      // Calcul de l'indice de départ pour la pagination
      int startIndex = (page - 1) * pageSize;

      // Récupération des produits avec pagination et chargement des catégories et de la marque
      var products = await query
          .Skip(startIndex)
          .Take(pageSize)
          .Include(p => p.ProductCategories)
          .ThenInclude(pc => pc.Category)
          .Include(p => p.Brand)
          .ToListAsync();

      // Si aucun produit n'est trouvé, renvoie NotFound
      if (products == null || products.Count == 0) {
        return NotFound();
      }

      // Ajout des en-têtes de réponse personnalisées
      Response.Headers.Add("X-Total-Pages", totalPages.ToString());
      Response.Headers.Add("X-Max-Price", maxProductPrice.ToString());
      Response.Headers.Add("X-Current-Page", page.ToString());

      // Projection des produits en un format spécifique avec les catégories
      var productsWithCategories = products.Select(p => new {
        p.Id,
        p.Label,
        p.Price,
        p.Description,
        p.Image_Url,
        p.Version,
        p.Brand,
        Categories = p.ProductCategories
              .Where(pc => pc.Category != null)
              .Select(pc => pc.Category?.Label)
              .ToList()
      });

      // Renvoie une réponse Ok avec les produits
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
        Categories = product.ProductCategories
          .Where(pc => pc.Category != null)
          .Select(pc => pc.Category?.Label)
          .ToList()
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
