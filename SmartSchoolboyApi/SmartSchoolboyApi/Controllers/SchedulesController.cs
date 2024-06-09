using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchoolboyApi.Models;
using System.Linq;

namespace SmartSchoolboyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchedulesController : ControllerBase
    {
        private readonly SmartSchoolboyBaseContext _context;

        public SchedulesController(SmartSchoolboyBaseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: api/Schedules
        /// </summary>
        /// <returns>Результат задачи содержит <see cref="List{T}"/> содержащий элементы последовательности <see cref="Schedule"/></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Schedule>>> GetSchedules()
        {
            try
            {
                if (_context.Schedules is null)
                    return NotFound();

                return Ok(await _context.Schedules.ToListAsync());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error, the server is not responding");
            }
        }

        /// <summary>
        /// GET: api/Schedules/5
        /// </summary>
        /// <param name="id">Параметр индификатора расписания</param>
        /// <returns>Результат задачи содержит найденый обьект <see cref="Schedule"/></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("{id}")]
        public async Task<ActionResult<Schedule>> GetSchedule(int id)
        {
            try
            {
                if (_context.Schedules is null)
                    return NotFound();

                var schedule = await _context.Schedules.FindAsync(id);

                if (schedule is null)
                    return NotFound();

                return Ok(schedule);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error, the server is not responding");
            }
        }

        /// <summary>
        /// GET: api/Schedules/search/5
        /// </summary>
        /// <param name="search">Параметр для поиска и фильтрации данных</param>
        /// <returns>Результат задачи содержит <see cref="List{T}"/> содержащий отфильтрованные элементы последовательности <see cref="Schedule"/></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("search/{search}")]
        public async Task<ActionResult<Schedule>> SearchSchedule(string search)
        {
            try
            {
                if (_context.Schedules is null)
                    return NotFound();

                var schedule = await _context.Schedules.ToListAsync();

                if (Int32.TryParse(search, out int groupId))
                    schedule = schedule.Where(p => p.GroupId.ToString().Contains(search)).ToList();
                else schedule = schedule.Where(p => p.Group.Name.ToLower().Trim().Contains(search.ToLower().Trim()) ||
                    p.Group.Course.Name.ToLower().Trim().Contains(search.ToLower().Trim())).ToList();

                //schedule = schedule.Where(p => p.Date >= DateTime.Today).ToList();

                return Ok(schedule);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error, the server is not responding");
            }
        }

        /// <summary>
        /// PUT: api/Schedules/5
        /// </summary>
        /// <param name="id">Параметр индификатора расписания</param>
        /// <param name="schedule">Параметр обьекта <see cref="Schedule"/></param>
        /// <returns>Результат задачи, изменение обьекта класса <see cref="Schedule"/></returns>
        /// <exception cref="DbUpdateConcurrencyException"></exception>
        /// <exception cref="Exception"></exception>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSchedule(int id, Schedule schedule)
        {
            try
            {
                if (id != schedule.Id)
                    return BadRequest();

                _context.Entry(schedule).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ScheduleExists(id))
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
        /// POST: api/Schedules
        /// </summary>
        /// <param name="schedule">Параметр обьекта <see cref="Schedule"/></param>
        /// <returns>Результат задачи, новый обьект класса <see cref="Schedule"/></returns>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        public async Task<ActionResult<Schedule>> PostSchedule(Schedule schedule)
        {
            try
            {
                var _group = await _context.Groups.FindAsync(schedule.GroupId);
                if (_group is null)
                    return BadRequest();

                await _context.AddAsync(new Schedule()
                {
                    Id = schedule.Id,
                    Group = _group,
                    Date = schedule.Date,
                    StartTime = schedule.StartTime,
                    EndTime = schedule.EndTime,
                });

                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(PostSchedule), new { id = schedule.Id }, schedule);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error, the server is not responding");
            }
        }

        /// <summary>
        /// DELETE: api/Schedules/5
        /// </summary>
        /// <param name="id">Параметр индификатора расписания</param>
        /// <returns>Результат задачи, удаление обьекта класса <see cref="Schedule"/></returns>
        /// <exception cref="Exception"></exception>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSchedule(int id)
        {
            try
            {
                if (_context.Schedules is null)
                    return NotFound();

                var schedule = await _context.Schedules.FindAsync(id);
                if (schedule is null)
                    return NotFound();

                _context.Schedules.Remove(schedule);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error, the server is not responding");
            }
        }

        private bool ScheduleExists(int id)
        {
            return (_context.Schedules?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
