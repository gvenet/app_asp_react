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
  public class LoginController : ControllerBase {
    private readonly ApiContext _context;

    public LoginController(ApiContext context) {
      _context = context;
    }

    // GET: api/Login
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Login>>> GetLogin() {
      if (_context.Logins == null) {
        return NotFound();
      }
      return await _context.Logins.ToListAsync();
    }

    // GET: api/Login/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Login>> GetLogin(long id) {
      if (_context.Logins == null) {
        return NotFound();
      }
      var login = await _context.Logins.FindAsync(id);

      if (login == null) {
        return NotFound();
      }

      return login;
    }

    // PUT: api/Login/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("{id}")]
    public async Task<IActionResult> PutLogin(long id, Login login) {
      if (id != login.Id) {
        return BadRequest();
      }

      _context.Entry(login).State = EntityState.Modified;

      try {
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException) {
        if (!LoginExists(id)) {
          return NotFound();
        }
        else {
          throw;
        }
      }

      return NoContent();
    }

    // POST: api/Login
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Login>> PostLogin(Login login) {
      if (_context.Logins == null) {
        return Problem("Entity set 'Context.Login'  is null.");
      }
      _context.Logins.Add(login);
      await _context.SaveChangesAsync();

      return CreatedAtAction("GetLogin", new { id = login.Id }, login);
    }

    // DELETE: api/Login/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLogin(long id) {
      if (_context.Logins == null) {
        return NotFound();
      }
      var login = await _context.Logins.FindAsync(id);
      if (login == null) {
        return NotFound();
      }

      _context.Logins.Remove(login);
      await _context.SaveChangesAsync();

      return NoContent();
    }

    private bool LoginExists(long id) {
      return (_context.Logins?.Any(e => e.Id == id)).GetValueOrDefault();
    }
  }
}
