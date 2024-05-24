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

        // GET: api/Attendances
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Attendance>>> GetAttendances()
        {
          if (_context.Attendances is null)
              return NotFound();

            return await _context.Attendances.ToListAsync();
        }

        // GET: api/Attendances/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Attendance>> GetAttendance(int id)
        {
          if (_context.Attendances is null)
              return NotFound();

            var attendance = await _context.Attendances.FindAsync(id);

            if (attendance is null)
                return NotFound();

            return attendance;
        }

        // PUT: api/Attendances/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAttendance(int id, Attendance attendance)
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

        // POST: api/Attendances
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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
            catch
            {
                return BadRequest();
            }
        }

        // DELETE: api/Attendances/5
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

                return NoContent();
            }
            catch
            {
                return BadRequest();
            }
        }

        private bool AttendanceExists(int id)
        {
            return (_context.Attendances?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
