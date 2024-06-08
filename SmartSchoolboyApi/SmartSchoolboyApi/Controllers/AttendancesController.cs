using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchoolboyApi.Models;

namespace SmartSchoolboyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AttendancesController : ControllerBase
    {
        private readonly SmartSchoolboyBaseContext _context;

        public AttendancesController(SmartSchoolboyBaseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: api/Attendances
        /// </summary>
        /// <returns>Результат задачи содержит <see cref="List{T}"/> содержащий элементы последовательности <see cref="Attendance"/></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Attendance>>> GetAttendances()
        {
            try
            {
                if (_context.Attendances is null)
                    return NotFound();

                return Ok(await _context.Attendances.ToListAsync());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error, the server is not responding");
            }
        }

        /// <summary>
        /// GET: api/Attendances/5
        /// </summary>
        /// <param name="id">Параметр индификатора посещаемости</param>
        /// <returns>Результат задачи содержит найденый обьект <see cref="Attendance"/></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("{id}")]
        public async Task<ActionResult<Attendance>> GetAttendance(int id)
        {
            try
            {
                if (_context.Attendances is null)
                    return NotFound();

                var attendance = await _context.Attendances.FindAsync(id);

                if (attendance is null)
                    return NotFound();

                return Ok(attendance);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error, the server is not responding");
            }
        }

        /// <summary>
        /// GET: api/Attendances/search/5
        /// </summary>
        /// <param name="search">Параметр для поиска и фильтрации данных</param>
        /// <returns>Результат задачи содержит <see cref="List{T}"/> содержащий отфильтрованные элементы последовательности <see cref="Attendance"/></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("search/{search}")]
        public async Task<ActionResult<Attendance>> SearchAttendance(string search)
        {
            try
            {
                if (_context.Attendances is null)
                    return NotFound();

                var attendance = await _context.Attendances.Where(p => p.Student.LastName.ToLower().Trim().Contains(search.ToLower().Trim()) ||
                    p.Student.FirstName.ToLower().Trim().Contains(search.ToLower().Trim()) ||
                    p.Student.Patronymic!.ToLower().Trim().Contains(search.ToLower().Trim()) ||
                    p.Schedule.Group.Name.ToLower().Trim().Contains(search.ToLower().Trim()) ||
                    p.Schedule.GroupId.ToString() == search).ToListAsync();

                return Ok(attendance);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error, the server is not responding");
            }
        }

        /// <summary>
        /// PUT: api/Attendances/5
        /// </summary>
        /// <param name="id">Параметр индификатора посещаемости</param>
        /// <param name="attendance">Параметр обьекта <see cref="Attendance"/></param>
        /// <returns>Результат задачи, изменение обьекта класса <see cref="Attendance"/></returns>
        /// <exception cref="DbUpdateConcurrencyException"></exception>
        /// <exception cref="Exception"></exception>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAttendance(int id, Attendance attendance)
        {
            try
            {
                if (id != attendance.Id)
                    return BadRequest();

                _context.Entry(attendance).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttendanceExists(id))
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
        /// POST: api/Attendances
        /// </summary>
        /// <param name="attendance">Параметр обьекта <see cref="Attendance"/></param>
        /// <returns>Результат задачи, новый обьект класса <see cref="Attendance"/></returns>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        public async Task<ActionResult<Attendance>> PostAttendance(Attendance attendance)
        {
            try
            {
                var _schedule = await _context.Schedules.FindAsync(attendance.ScheduleId);
                var _student = await _context.Students.FindAsync(attendance.StudentId);

                if (_schedule is null || _student is null)
                    return BadRequest();

                await _context.AddAsync(new Attendance()
                {
                    Id = attendance.Id,
                    Student = _student,
                    Schedule = _schedule,
                    MarkOfPresence = attendance.MarkOfPresence,
                });

                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(PostAttendance), new { id = attendance.Id }, attendance);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error, the server is not responding");
            }
        }

        /// <summary>
        /// DELETE: api/Attendances/5
        /// </summary>
        /// <param name="id">Параметр индификатора посещаемости</param>
        /// <returns>Результат задачи, удаление обьекта класса <see cref="Attendance"/></returns>
        /// <exception cref="Exception"></exception>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAttendance(int id)
        {
            try
            {
                if (_context.Attendances is null)
                    return NotFound();

                var attendance = await _context.Attendances.FindAsync(id);
                if (attendance is null)
                    return NotFound();

                _context.Attendances.Remove(attendance);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error, the server is not responding");
            }
        }

        private bool AttendanceExists(int id)
        {
            return (_context.Attendances?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
