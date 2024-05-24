using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchoolboyApi.Models;

namespace SmartSchoolboyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchoolSubjectsController : ControllerBase
    {
        private readonly SmartSchoolboyBaseContext _context;

        public SchoolSubjectsController(SmartSchoolboyBaseContext context)
        {
            _context = context;
        }

        // GET: api/SchoolSubjects
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SchoolSubject>>> GetSchoolSubjects()
        {
          if (_context.SchoolSubjects is null)
              return NotFound();

            return await _context.SchoolSubjects.ToListAsync();
        }

        // GET: api/SchoolSubjects/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SchoolSubject>> GetSchoolSubject(int id)
        {
          if (_context.SchoolSubjects is null)
              return NotFound();

            var schoolSubject = await _context.SchoolSubjects.FindAsync(id);

            if (schoolSubject is null)
                return NotFound();

            return schoolSubject;
        }

        // GET: api/SchoolSubjects/5
        [HttpGet("search/{search}")]
        public async Task<ActionResult<SchoolSubject>> SearchSchoolSubject(string search)
        {
            try
            {
                if (_context.SchoolSubjects is null)
                    return NotFound();

                var schoolSubject = await _context.SchoolSubjects.Where(p => p.Name.ToLower().Trim().Contains(search.ToLower().Trim())).ToListAsync();

                return Ok(schoolSubject);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error, the server is not responding");
            }
        }

        // PUT: api/SchoolSubjects/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSchoolSubject(int id, SchoolSubject schoolSubject)
        {
            if (id != schoolSubject.Id)
                return BadRequest();

            _context.Entry(schoolSubject).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SchoolSubjectExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/SchoolSubjects
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SchoolSubject>> PostSchoolSubject(SchoolSubject schoolSubject)
        {
            try
            {
                await _context.SchoolSubjects.AddAsync(new SchoolSubject()
                {
                    Id = schoolSubject.Id,
                    Name = schoolSubject.Name,
                });

                await _context.SaveChangesAsync();

                return CreatedAtAction("GetSchoolSubject", new { id = schoolSubject.Id }, schoolSubject);
            }
            catch
            {
                return BadRequest();
            }
        }

        // DELETE: api/SchoolSubjects/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchoolSubject(int id)
        {
            try
            {
                if (_context.SchoolSubjects is null)
                    return NotFound();

                var schoolSubject = await _context.SchoolSubjects.FindAsync(id);
                if (schoolSubject is null)
                    return NotFound();

                _context.SchoolSubjects.Remove(schoolSubject);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch
            {
                return BadRequest();
            }
        }

        private bool SchoolSubjectExists(int id)
        {
            return (_context.SchoolSubjects?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
