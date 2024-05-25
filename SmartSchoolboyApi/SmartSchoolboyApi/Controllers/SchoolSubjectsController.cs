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

        /// <summary>
        /// GET: api/SchoolSubjects
        /// </summary>
        /// <returns>Результат задачи содержит <see cref="List{T}"/> содержащий элементы последовательности <see cref="SchoolSubject"/></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SchoolSubject>>> GetSchoolSubjects()
        {
            try
            {
                if (_context.SchoolSubjects is null)
                    return NotFound();

                return Ok(await _context.SchoolSubjects.ToListAsync());
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// GET: api/SchoolSubjects/5
        /// </summary>
        /// <param name="id">Параметр индификатора учебного предмета</param>
        /// <returns>Результат задачи содержит найденый обьект <see cref="SchoolSubject"/></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("{id}")]
        public async Task<ActionResult<SchoolSubject>> GetSchoolSubject(int id)
        {
            try
            {
                if (_context.SchoolSubjects is null)
                    return NotFound();

                var schoolSubject = await _context.SchoolSubjects.FindAsync(id);

                if (schoolSubject is null)
                    return NotFound();

                return Ok(schoolSubject);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error, the server is not responding");
            }
        }

        /// <summary>
        /// GET: api/SchoolSubjects/search/5
        /// </summary>
        /// <param name="search">Параметр для поиска и фильтрации данных</param>
        /// <returns>Результат задачи содержит <see cref="List{T}"/> содержащий отфильтрованные элементы последовательности <see cref="SchoolSubject"/></returns>
        /// <exception cref="Exception"></exception>
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

        /// <summary>
        /// PUT: api/SchoolSubjects/5
        /// </summary>
        /// <param name="id">Параметр индификатора учебного предмета</param>
        /// <param name="schoolSubject">Параметр обьекта <see cref="SchoolSubject"/></param>
        /// <returns>Результат задачи, изменение обьекта класса <see cref="SchoolSubject"/></returns>
        /// <exception cref="DbUpdateConcurrencyException"></exception>
        /// <exception cref="Exception"></exception>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSchoolSubject(int id, SchoolSubject schoolSubject)
        {
            try
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
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error, the server is not responding");
            }
        }

        /// <summary>
        /// POST: api/SchoolSubjects
        /// </summary>
        /// <param name="schoolSubject">Параметр обьекта <see cref="SchoolSubject"/></param>
        /// <returns>Результат задачи, новый обьект класса <see cref="SchoolSubject"/></returns>
        /// <exception cref="Exception"></exception>
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

                return CreatedAtAction(nameof(PostSchoolSubject), new { id = schoolSubject.Id }, schoolSubject);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error, the server is not responding");
            }
        }

        /// <summary>
        /// DELETE: api/SchoolSubjects/5
        /// </summary>
        /// <param name="id">Параметр индификатора учебного предмета</param>
        /// <returns>Результат задачи, удаление обьекта класса <see cref="SchoolSubject"/></returns>
        /// <exception cref="Exception"></exception>
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

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error, the server is not responding");
            }
        }

        private bool SchoolSubjectExists(int id)
        {
            return (_context.SchoolSubjects?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
