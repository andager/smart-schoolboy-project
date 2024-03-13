using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchoolboyApi.Models;

namespace SmartSchoolboyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GendersController : ControllerBase
    {
        private readonly SmartSchoolboyBaseContext _context;

        public GendersController(SmartSchoolboyBaseContext context)
        {
            _context = context;
        }

        // GET: api/Genders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Gender>>> GetGenders()
        {
          if (_context.Genders is null)
              return NotFound();

            return await _context.Genders.ToListAsync();
        }

        // GET: api/Genders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Gender>> GetGender(int id)
        {
          if (_context.Genders is null)
              return NotFound();

            var gender = await _context.Genders.FindAsync(id);

            if (gender is null)
                return NotFound();

            return gender;
        }

        // PUT: api/Genders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGender(int id, Gender gender)
        {
            if (id != gender.Id)
                return BadRequest();

            _context.Entry(gender).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GenderExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/Genders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Gender>> PostGender(Gender gender)
        {
            try
            {
                await _context.AddAsync(new Gender()
                {
                    Id = gender.Id,
                    Name = gender.Name,
                });

                await _context.SaveChangesAsync();

                return CreatedAtAction("GetGender", new { id = gender.Id }, gender);
            }
            catch
            {
                return BadRequest();
            }
        }

        // DELETE: api/Genders/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGender(int id)
        {
            try
            {
                if (_context.Genders is null)
                    return NotFound();

                var gender = await _context.Genders.FindAsync(id);
                if (gender is null)
                    return NotFound();

                _context.Genders.Remove(gender);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch
            {
                return BadRequest();
            }
        }

        private bool GenderExists(int id)
        {
            return (_context.Genders?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
