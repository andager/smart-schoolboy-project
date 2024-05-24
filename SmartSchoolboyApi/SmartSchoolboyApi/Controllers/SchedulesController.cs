using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchoolboyApi.Models;

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

        // GET: api/Schedules
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Schedule>>> GetSchedules()
        {
          if (_context.Schedules is null)
              return NotFound();

            return await _context.Schedules.ToListAsync();
        }

        // GET: api/Schedules/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Schedule>> GetSchedule(int id)
        {
          if (_context.Schedules is null)
              return NotFound();

            var schedule = await _context.Schedules.FindAsync(id);

            if (schedule is null)
                return NotFound();

            return schedule;
        }

        // PUT: api/Schedules/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSchedule(int id, Schedule schedule)
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

        // POST: api/Schedules
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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
            catch
            {
                return BadRequest();
            }
        }

        // DELETE: api/Schedules/5
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

                return NoContent();
            }
            catch
            {
                return BadRequest();
            }
        }

        private bool ScheduleExists(int id)
        {
            return (_context.Schedules?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
