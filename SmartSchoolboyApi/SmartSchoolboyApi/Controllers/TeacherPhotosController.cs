using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchoolboyApi.Models;

namespace SmartSchoolboyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherPhotosController : ControllerBase
    {
        private readonly SmartSchoolboyBaseContext _context;

        public TeacherPhotosController(SmartSchoolboyBaseContext context)
        {
            _context = context;
        }

        // GET: api/Teachers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeacherPhoto>>> GetTeacherPhotos()
        {
            if (_context.TeacherPhotos is null)
                return NotFound();

            return await _context.TeacherPhotos.ToListAsync();
        }

        // GET: api/Teachers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TeacherPhoto>> GetTeacherPhoto(int id)
        {
            if (_context.TeacherPhotos is null)
                return NotFound();

            var teacherPhoto = await _context.TeacherPhotos.FindAsync(id);

            if (teacherPhoto is null)
                return NotFound();

            return teacherPhoto;
        }

        // PUT: api/Teachers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeacherPhoto(int id, TeacherPhoto teacherPhoto)
        {
            if (id != teacherPhoto.Id)
                return BadRequest();

            _context.Entry(teacherPhoto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                //if (!teacherPhoto(id))
                    return NotFound();
                //else
                //    throw;
            }

            return NoContent();
        }

        // POST: api/Teachers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TeacherPhoto>> PostTeacherPhoto(TeacherPhoto teacherPhoto)
        {
            try
            {
                await _context.TeacherPhotos.AddAsync(new TeacherPhoto()
                {
                    Photo = teacherPhoto.Photo
                });

                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(PostTeacherPhoto), new { id = teacherPhoto.Id }, teacherPhoto);
            }
            catch
            {
                return BadRequest();
            }
        }

        // DELETE: api/Teachers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeacherPhoto(int id)
        {
            try
            {
                if (_context.TeacherPhotos is null)
                    return NotFound();

                var teacherPhoto = await _context.TeacherPhotos.FindAsync(id);
                if (teacherPhoto is null)
                    return NotFound();

                _context.TeacherPhotos.Remove(teacherPhoto);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch
            {
                return BadRequest();
            }
        }

        private bool TeacherPhotoExists(int id)
        {
            return (_context.TeacherPhotos?.Any(e => e.Id == id)).GetValueOrDefault();
        }

    }
}
