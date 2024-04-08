using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchoolboyApi.Models;

namespace SmartSchoolboyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly SmartSchoolboyBaseContext _context;

        public StudentsController(SmartSchoolboyBaseContext context)
        {
            _context = context;
        }

        // GET: api/Students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
          if (_context.Students is null)
              return NotFound();

            return await _context.Students.ToListAsync();
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
          if (_context.Students is null)
              return NotFound();
            var student = await _context.Students.FindAsync(id);

            if (student is null)
                return NotFound();

            return student;
        }

        // PUT: api/Students/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, Student student)
        {
            if (id != student.Id)
                return BadRequest();

            _context.Entry(student).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/Students
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
            try
            {
                var _gender = await _context.Genders.FindAsync(student.GenderId);

                if (_gender is null)
                    return BadRequest();

                await _context.AddAsync(new Student
                {
                    Id = student.Id,
                    LastName = student.LastName,
                    FirstName = student.FirstName,
                    Patronymic = student.Patronymic,
                    DateOfBirch = student.DateOfBirch,
                    Gender = _gender,
                    NumberPhone = student.NumberPhone,
                    TelegramId = student.TelegramId,
                    IsActive = student.IsActive,
                });

                await _context.SaveChangesAsync();

                return CreatedAtAction("GetStudent", new { id = student.Id }, student);
            }
            catch
            {
                return BadRequest();
            }
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            try
            {
                if (_context.Students is null)
                    return NotFound();

                var student = await _context.Students.FindAsync(id);
                if (student is null)
                    return NotFound();

                _context.Students.Remove(student);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch
            {
                return BadRequest();
            }
        }

        private bool StudentExists(int id)
        {
            return (_context.Students?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
