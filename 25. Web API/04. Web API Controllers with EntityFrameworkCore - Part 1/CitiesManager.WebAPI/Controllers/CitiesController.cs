using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CitiesManager.WebAPI.DatabaseContext;
using CitiesManager.WebAPI.Models;

namespace CitiesManager.WebAPI.Controllers
{
 [Route("api/[controller]")]
 [ApiController]
 public class CitiesController : ControllerBase
 {
  private readonly ApplicationDbContext _context;

  public CitiesController(ApplicationDbContext context)
  {
   _context = context;
  }

  // GET: api/Cities
  [HttpGet]
  public async Task<ActionResult<IEnumerable<City>>> GetCities()
  {
   if (_context.Cities == null)
   {
    return NotFound();
   }
   return await _context.Cities.ToListAsync();
  }


  // GET: api/Cities/5
  [HttpGet("{id}")]
  public async Task<ActionResult<City>> GetCity(Guid id)
  {
   if (_context.Cities == null)
   {
    return NotFound();
   }
   var city = await _context.Cities.FindAsync(id);

   if (city == null)
   {
    return NotFound();
   }

   return city;
  }


  // PUT: api/Cities/5
  // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
  [HttpPut("{id}")]
  public async Task<IActionResult> PutCity(Guid id, City city)
  {
   if (id != city.CityID)
   {
    return BadRequest();
   }

   _context.Entry(city).State = EntityState.Modified;

   try
   {
    await _context.SaveChangesAsync();
   }
   catch (DbUpdateConcurrencyException)
   {
    if (!CityExists(id))
    {
     return NotFound();
    }
    else
    {
     throw;
    }
   }

   return NoContent();
  }


  // POST: api/Cities
  // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
  [HttpPost]
  public async Task<ActionResult<City>> PostCity(City city)
  {
   if (_context.Cities == null)
   {
    return Problem("Entity set 'ApplicationDbContext.Cities'  is null.");
   }
   _context.Cities.Add(city);
   await _context.SaveChangesAsync();

   return CreatedAtAction("GetCity", new { id = city.CityID }, city);
  }


  // DELETE: api/Cities/5
  [HttpDelete("{id}")]
  public async Task<IActionResult> DeleteCity(Guid id)
  {
   if (_context.Cities == null)
   {
    return NotFound();
   }
   var city = await _context.Cities.FindAsync(id);
   if (city == null)
   {
    return NotFound();
   }

   _context.Cities.Remove(city);
   await _context.SaveChangesAsync();

   return NoContent();
  }

  private bool CityExists(Guid id)
  {
   return (_context.Cities?.Any(e => e.CityID == id)).GetValueOrDefault();
  }
 }
}
