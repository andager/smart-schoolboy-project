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

        /// <summary>
        /// POST: api/TeacherAutches
        /// </summary>
        /// <param name="teacherAutch">Параметр обьекта <see cref="TeacherAutch"/></param>
        /// <returns>Результат задачи содержит авторизовавшегося пользователя</returns>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        public async Task<ActionResult<TeacherAutch>> AutchTeacherAutch(TeacherAutch teacherAutch)
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
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error, the server is not responding");
            }
        }
    }
}
