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

        /// <summary>
        /// GET: api/Students
        /// </summary>
        /// <returns>Результат задачи содержит <see cref="List{T}"/> содержащий элементы последовательности <see cref="Student"/></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
            try
            {
                if (_context.Students is null)
                    return NotFound();

                return Ok(await _context.Students.ToListAsync());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error, the server is not responding");
            }
        }

        /// <summary>
        /// GET: api/Students/5
        /// </summary>
        /// <param name="id">Параметр индификатора ученика</param>
        /// <returns>Результат задачи содержит найденый обьект <see cref="Student"/></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            try
            {
                if (_context.Students is null)
                    return NotFound();
                var student = await _context.Students.FindAsync(id);

                if (student is null)
                    return NotFound();

                return Ok(student);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error, the server is not responding");
            }
        }

        /// <summary>
        /// GET: api/Students/search/5
        /// </summary>
        /// <param name="search">Параметр для поиска и фильтрации данных</param>
        /// <returns>Результат задачи содержит <see cref="List{T}"/> содержащий отфильтрованные элементы последовательности <see cref="Student"/></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("search/{search}")]
        public async Task<ActionResult<Student>> SearchStudent(string search)
        {
            try
            {
                if (_context.Students is null)
                    return NotFound();

                var student = await _context.Students.Where(p => p.LastName.ToLower().Trim().Contains(search.ToLower().Trim()) ||
                p.FirstName.ToLower().Trim().Contains(search.ToLower().Trim()) ||
                p.Patronymic!.ToLower().Trim().Contains(search.ToLower().Trim()) ||
                p.Gender.Name.ToLower().Trim().Contains(search.ToLower().Trim()) ||
                p.NumberPhone!.ToLower().Trim().Contains(search.ToLower().Trim())).ToListAsync();

                return Ok(student);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error, the server is not responding");
            }
        }

        /// <summary>
        /// PUT: api/Students/5
        /// </summary>
        /// <param name="id">Параметр индификатора ученика</param>
        /// <param name="student">Параметр обьекта <see cref="Student"/></param>
        /// <returns>Результат задачи, изменение обьекта класса <see cref="Student"/></returns>
        /// <exception cref="DbUpdateConcurrencyException"></exception>
        /// <exception cref="Exception"></exception>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutStudent(int id, Student student)
        {
            try
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
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error, the server is not responding");
            }
        }

        /// <summary>
        /// POST: api/Students
        /// </summary>
        /// <param name="student">Параметр обьекта <see cref="Student"/></param>
        /// <returns>Результат задачи, новый обьект класса <see cref="Student"/></returns>
        /// <exception cref="Exception"></exception>
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

                return CreatedAtAction(nameof(PostStudent), new { id = student.Id }, student);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error, the server is not responding");
            }
        }

        /// <summary>
        /// POST: api/Students/groupId
        /// </summary>
        /// <param name="student">Параметр обьекта <see cref="Student"/></param>
        /// <param name="groupId"></param>
        /// <returns>Результат задачи, новый обьект класса <see cref="Student"/></returns>
        /// <exception cref="ArgumentNullException"></exception>
        [HttpPost("{groupId}")]
        public async Task<ActionResult<Student>> PostThemeByLesson(Student student, int groupId)
        {
            try
            {
                var _gender = await _context.Genders.FindAsync(student.GenderId);

                if (_gender is null)
                    return BadRequest();

                var studentG = await _context.AddAsync(new Student()
                {
                    Id = student.Id,
                    LastName = student.LastName,
                    FirstName = student.FirstName,
                    Patronymic = student.Patronymic,
                    DateOfBirch = student.DateOfBirch,
                    GenderId = student.GenderId,
                    Gender = _gender,
                    NumberPhone = student.NumberPhone,
                    TelegramId = student.TelegramId,
                    IsActive = student.IsActive

                });

                (await _context.Groups.FindAsync(groupId) ?? throw new ArgumentNullException()).Students.Add(student);

                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(PostStudent), new { id = studentG.Entity.Id }, studentG.Entity);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error, the server is not responding");
            }
        }

        /// <summary>
        /// DELETE: api/Students/5
        /// </summary>
        /// <param name="id">Параметр индификатора ученика</param>
        /// <returns>Результат задачи, удаление обьекта класса <see cref="Student"/></returns>
        /// <exception cref="Exception"></exception>
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

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error, the server is not responding");
            }
        }

        private bool StudentExists(int id)
        {
            return (_context.Students?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
