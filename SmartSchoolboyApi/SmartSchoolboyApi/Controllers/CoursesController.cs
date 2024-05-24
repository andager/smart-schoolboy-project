using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchoolboyApi.Models;

namespace SmartSchoolboyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly SmartSchoolboyBaseContext _context;

        public CoursesController(SmartSchoolboyBaseContext context)
        {
            _context = context;
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CourseDTO>>> GetCourses()
        {
            try
            {
                if (_context.Courses is null)
                    return NotFound();

                return Ok(await _context.Courses.ToListAsync());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error, the server is not responding");
            }
        }

        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            try
            {
                if (_context.Courses is null)
                    return NotFound();

                var course = await _context.Courses.FindAsync(id);

                if (course is null)
                    return NotFound();

                return Ok(course);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error, the server is not responding");
            }
        }

        [HttpGet("search/{search}")]
        public async Task<ActionResult<Course>> SearchCourse(string search)
        {
            try
            {
                if (_context.Courses is null)
                    return NotFound();

                var course = await _context.Courses.Where(p => p.Name.ToLower().Trim().Contains(search.ToLower().Trim())).ToListAsync();

                return Ok(course);

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error, the server is not responding");
            }
        }
        // PUT: api/Courses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCourse(int id, Course course)
        {
            try
            {
                if (id != course.Id)
                    return BadRequest();

                _context.Entry(course).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(id))
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

        // POST: api/Courses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
            try
            {
                var _teacher = await _context.Teachers.FindAsync(course.TeacherId);

                if (_teacher is null)
                    return BadRequest();

                await _context.AddAsync(new Course()
                {
                    Id = course.Id,
                    Name = course.Name,
                    Teacher = _teacher,
                    IsActive = course.IsActive,
                });

                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(PostCourse), new { id = course.Id }, course);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error, the server is not responding");
            }
        }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            try
            {
                if (_context.Courses is null)
                    return NotFound();

                var course = await _context.Courses.FindAsync(id);
                if (course is null)
                    return NotFound();

                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error, the server is not responding");
            }
        }

        private bool CourseExists(int id)
        {
            return (_context.Courses?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
