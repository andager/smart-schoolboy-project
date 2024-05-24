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

        /// <summary>
        /// GET: api/TeacherPhotos
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TeacherPhoto>>> GetTeacherPhotos()
        {
            try
            {
                if (_context.TeacherPhotos is null)
                    return NotFound();

                return Ok(await _context.TeacherPhotos.ToListAsync());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error, the server is not responding");
            }
        }

        /// <summary>
        /// GET: api/TeacherPhotos/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<TeacherPhoto>> GetTeacherPhoto(int id)
        {
            try
            {
                if (_context.TeacherPhotos is null)
                    return NotFound();

                var teacherPhoto = await _context.TeacherPhotos.FindAsync(id);

                if (teacherPhoto is null)
                    return NotFound();

                return Ok(teacherPhoto);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error, the server is not responding");
            }
        }

        /// <summary>
        /// PUT: api/TeacherPhotos/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="teacherPhoto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeacherPhoto(int id, TeacherPhoto teacherPhoto)
        {
            try
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
                    if (!TeacherPhotoExists(id))
                        return NotFound();
                    else
                        throw;
                }

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error, the server is not responding");
            }
        }

        /// <summary>
        /// POST: api/TeacherPhotos
        /// </summary>
        /// <param name="teacherPhoto"></param>
        /// <returns></returns>
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
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error, the server is not responding");
            }
        }

        /// <summary>
        /// DELETE: api/TeacherPhotos/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
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
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error, the server is not responding");
            }
        }

        private bool TeacherPhotoExists(int id)
        {
            return (_context.TeacherPhotos?.Any(e => e.Id == id)).GetValueOrDefault();
        }

    }
}
