using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchoolboyApi.Models;

namespace SmartSchoolboyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControlThemePlanesController : ControllerBase
    {
        private readonly SmartSchoolboyBaseContext _context;

        public ControlThemePlanesController(SmartSchoolboyBaseContext context)
        {
            _context = context;
        }

        // GET: api/ControlThemePlanes
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ControlThemePlane>>> GetControlThemePlanes()
        {
          if (_context.ControlThemePlanes is null)
              return NotFound();

            return await _context.ControlThemePlanes.ToListAsync();
        }

        // GET: api/ControlThemePlanes/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ControlThemePlane>> GetControlThemePlane(int id)
        {
          if (_context.ControlThemePlanes is null)
              return NotFound();

            var controlThemePlane = await _context.ControlThemePlanes.FindAsync(id);

            if (controlThemePlane is null)
                return NotFound();

            return controlThemePlane;
        }

        // PUT: api/ControlThemePlanes/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutControlThemePlane(int id, ControlThemePlane controlThemePlane)
        {
            if (id != controlThemePlane.Id)
                return BadRequest();

            _context.Entry(controlThemePlane).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ControlThemePlaneExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/ControlThemePlanes
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ControlThemePlane>> PostControlThemePlane(ControlThemePlane controlThemePlane)
        {
            try
            {
                await _context.AddAsync(new ControlThemePlane()
                {
                    Id = controlThemePlane.Id,
                    LessonName = controlThemePlane.LessonName,
                    LessonDescription = controlThemePlane.LessonDescription,
                });

                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(PostControlThemePlane), new { id = controlThemePlane.Id }, controlThemePlane);
            }
            catch
            {
                return BadRequest();
            }
        }

        // DELETE: api/ControlThemePlanes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteControlThemePlane(int id)
        {
            try
            {
                if (_context.ControlThemePlanes is null)
                    return NotFound();

                var controlThemePlane = await _context.ControlThemePlanes.FindAsync(id);
                if (controlThemePlane is null)
                    return NotFound();

                _context.ControlThemePlanes.Remove(controlThemePlane);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch
            {
                return BadRequest();
            }
        }

        private bool ControlThemePlaneExists(int id)
        {
            return (_context.ControlThemePlanes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
