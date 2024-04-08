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

        // GET: api/Teachers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Teacher>>> GetTeachers()
        {
            if (_context.Teachers is null)
                return NotFound();

            return await _context.Teachers.ToListAsync();
        }

        // GET: api/Teachers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Teacher>> GetTeacher(int id)
        {
            if (_context.Teachers is null)
                return NotFound();

            var teacher = await _context.Teachers.FindAsync(id);

            if (teacher is null)
                return NotFound();

            return teacher;
        }

        // PUT: api/Teachers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTeacher(int id, Teacher teacher)
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

        // POST: api/Teachers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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

                return CreatedAtAction("GetTeacher", new { id = teacher.Id }, teacher);
            }
            catch
            {
                return BadRequest();
            }
        }

        // DELETE: api/Teachers/5
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
            catch
            {
                return BadRequest();
            }
        }

        private bool TeacherExists(int id)
        {
            return (_context.Teachers?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
