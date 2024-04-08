using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchoolboyApi.Models;

namespace SmartSchoolboyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeacherAutchesController : ControllerBase
    {
        private readonly SmartSchoolboyBaseContext _context;

        public TeacherAutchesController(SmartSchoolboyBaseContext context)
        {
            _context = context;
        }

        // POST: api/TeacherAutches
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TeacherAutch>> PostTeacherAutch(TeacherAutch teacherAutch)
        {
            try
            {
                if (await _context.Teachers.FirstOrDefaultAsync(p => p.NumberPhone == teacherAutch.NumberPhone && p.Password == teacherAutch.Password) is Teacher currentTeacher)
                    return Ok(currentTeacher);
                else
                    return NotFound();
            }
            catch
            {   
                return BadRequest();
            }
        }
    }
}
