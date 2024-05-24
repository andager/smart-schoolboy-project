using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchoolboyApi.Models;

namespace SmartSchoolboyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeachersController : ControllerBase
    {
        private readonly SmartSchoolboyBaseContext _context;

        public TeachersController(SmartSchoolboyBaseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: api/Teachers
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Teacher>>> GetTeachers()
        {
            try
            {
                if (_context.Teachers is null)
                    return NotFound();

                return await _context.Teachers.ToListAsync();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error, the server is not responding");
            }
        }

        /// <summary>
        /// GET: api/Teachers/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Teacher>> GetTeacher(int id)
        {
            try
            {
                if (_context.Teachers is null)
                    return NotFound();

                var teacher = await _context.Teachers.FindAsync(id);

                if (teacher is null)
                    return NotFound();

                return Ok(teacher);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error, the server is not responding");
            }
        }

        /// <summary>
        /// GET: api/Teachers/search
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        [HttpGet("search/{search}")]
        public async Task<ActionResult<Teacher>> SearchTeachers(string search)
        {
            try
            {
                if (_context.Teachers is null)
                    return NotFound();

                var teacher = await _context.Teachers.Where(p => p.LastName.ToLower().Trim().Contains(search.ToLower().Trim()) ||
                    p.FirstName.ToLower().Trim().Contains(search.ToLower().Trim()) ||
                    p.Patronymic.ToLower().Trim().Contains(search.ToLower().Trim()) ||
                    p.Gender.Name.ToLower().Trim().Contains(search.ToLower().Trim()) ||
                    p.Role.Name.ToLower().Trim().Contains(search.ToLower().Trim())).ToListAsync();

                return Ok(teacher);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error, the server is not responding");
            }
        }

        /// <summary>
        /// PUT: api/Teachers/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="teacher"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeacher(int id, Teacher teacher)
        {
            try
            {
                if (id != teacher.Id)
                    return BadRequest();

                _context.Entry(teacher).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherExists(id))
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
        /// POST: api/Teachers
        /// </summary>
        /// <param name="teacher"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<Teacher>> PostTeacher(Teacher teacher)
        {
            try
            {
                var _gender = await _context.Genders.FindAsync(teacher.GenderId);
                var _role = await _context.Roles.FindAsync(teacher.RoleId);

                if (_gender is null || _role is null)
                    return BadRequest();

                await _context.Teachers.AddAsync(new Teacher()
                {
                    Id = teacher.Id,
                    LastName = teacher.LastName,
                    FirstName = teacher.FirstName,
                    Patronymic = teacher.Patronymic,
                    NumberPhone = teacher.NumberPhone,
                    Password = teacher.Password,
                    Gender = _gender,
                    DateOfBirtch = teacher.DateOfBirtch,
                    Role = _role,
                    WorkExperience = teacher.WorkExperience,
                    TeacherPhoto = teacher.TeacherPhoto,
                    IsActive = teacher.IsActive,
                });

                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(PostTeacher), new { id = teacher.Id }, teacher);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error, the server is not responding");
            }
        }

        /// <summary>
        /// DELETE: api/Teachers/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTeacher(int id)
        {
            try
            {
                if (_context.Teachers is null)
                    return NotFound();

                var teacher = await _context.Teachers.FindAsync(id);
                if (teacher is null)
                    return NotFound();

                _context.Teachers.Remove(teacher);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error, the server is not responding");
            }
        }

        private bool TeacherExists(int id)
        {
            return (_context.Teachers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
