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

        /// <summary>
        /// GET: api/Courses
        /// </summary>
        /// <returns>Результат задачи содержит <see cref="List{T}"/> содержащий элементы последовательности <see cref="Course"/></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
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

        /// <summary>
        /// GET: api/Courses/5
        /// </summary>
        /// <param name="id">Параметр индификатора курса</param>
        /// <returns>Результат задачи содержит найденый обьект <see cref="Course"/></returns>
        /// <exception cref="Exception"></exception>
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

        /// <summary>
        /// GET: api/Courses/search/5
        /// </summary>
        /// <param name="search">Параметр для поиска и фильтрации данных</param>
        /// <returns>Результат задачи содержит <see cref="List{T}"/> содержащий отфильтрованные элементы последовательности <see cref="Course"/></returns>
        /// <exception cref="Exception"></exception>
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

        /// <summary>
        /// PUT: api/Courses/5
        /// </summary>
        /// <param name="id">Параметр индификатора курса</param>
        /// <param name="course">Параметр обьекта <see cref="Course"/></param>
        /// <returns>Результат задачи, изменение обьекта класса <see cref="Course"/></returns>
        /// <exception cref="DbUpdateConcurrencyException"></exception>
        /// <exception cref="Exception"></exception>
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

        /// <summary>
        /// POST: api/Courses
        /// </summary>
        /// <param name="course">Параметр обьекта <see cref="Course"/></param>
        /// <returns>Результат задачи, новый обьект класса <see cref="Course"/></returns>
        /// <exception cref="Exception"></exception>
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

        /// <summary>
        /// DELETE: api/Courses/5
        /// </summary>
        /// <param name="id">Параметр индификатора курса</param>
        /// <returns>Результат задачи, удаление обьекта класса <see cref="Course"/></returns>
        /// <exception cref="Exception"></exception>
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

                return Ok();
            }
            catch (Exception)
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
