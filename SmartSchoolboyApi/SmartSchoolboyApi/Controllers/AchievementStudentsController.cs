using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchoolboyApi.Models;

namespace SmartSchoolboyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AchievementStudentsController : ControllerBase
    {
        private readonly SmartSchoolboyBaseContext _context;

        public AchievementStudentsController(SmartSchoolboyBaseContext context)
        {
            _context = context;
        }

        // GET: api/AchievementStudents
        [HttpGet]
        public async Task<ActionResult<IEnumerable<AchievementStudent>>> GetAchievementStudents()
        {
          if (_context.AchievementStudents is null)
              return NotFound();

            return await _context.AchievementStudents.ToListAsync();
        }

        // GET: api/AchievementStudents/5
        [HttpGet("{id}")]
        public async Task<ActionResult<AchievementStudent>> GetAchievementStudent(int id)
        {
          if (_context.AchievementStudents is null)
              return NotFound();

            var achievementStudent = await _context.AchievementStudents.FindAsync(id);

            if (achievementStudent is null)
                return NotFound();

            return achievementStudent;
        }

        // PUT: api/AchievementStudents/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutAchievementStudent(int id, AchievementStudent achievementStudent)
        {
            if (id != achievementStudent.Id)
                return BadRequest();

            _context.Entry(achievementStudent).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AchievementStudentExists(id))
                    return NotFound();
                else
                    throw;
            }

            return NoContent();
        }

        // POST: api/AchievementStudents
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<AchievementStudent>> PostAchievementStudent(AchievementStudent achievementStudent)
        {
            try
            {
                var _course = await _context.Courses.FindAsync(achievementStudent.CourseId);
                var _student = await _context.Students.FindAsync(achievementStudent.StudentId);

                if (_course is null || _student is null)
                    return BadRequest();

                await _context.AddAsync(new AchievementStudent()
                {
                    Id = achievementStudent.Id,
                    Student = _student,
                    Course = _course,
                    Photo = achievementStudent.Photo,
                });

                await _context.SaveChangesAsync();  

                return CreatedAtAction("GetAchievementStudent", new { id = achievementStudent.Id }, achievementStudent);
            }
            catch
            {
                return BadRequest();
            }
        }

        // DELETE: api/AchievementStudents/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAchievementStudent(int id)
        {
            try
            {
                if (_context.AchievementStudents is null)
                    return NotFound();

                var achievementStudent = await _context.AchievementStudents.FindAsync(id);
                if (achievementStudent is null)
                    return NotFound();

                _context.AchievementStudents.Remove(achievementStudent);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch
            {
                return BadRequest();
            }
        }

        private bool AchievementStudentExists(int id)
        {
            return (_context.AchievementStudents?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
