using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchoolboyApi.Models;

namespace SmartSchoolboyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupsController : ControllerBase
    {
        private readonly SmartSchoolboyBaseContext _context;

        public GroupsController(SmartSchoolboyBaseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: api/Groups
        /// </summary>
        /// <returns>Результат задачи содержит <see cref="List{T}"/> содержащий элементы последовательности <see cref="Group"/></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Group>>> GetGroups()
        {
            try
            {
                if (_context.Groups is null)
                    return NotFound();

                return Ok(await _context.Groups.ToListAsync());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error, the server is not responding");
            }
        }

        /// <summary>
        /// GET: api/Groups/5
        /// </summary>
        /// <param name="id">Параметр индификатора группы</param>
        /// <returns>Результат задачи содержит найденый обьект <see cref="Group"/></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("{id}")]
        public async Task<ActionResult<Group>> GetGroup(int id)
        {
            try
            {
                if (_context.Groups is null)
                    return NotFound();

                var @group = await _context.Groups.FindAsync(id);

                if (@group is null)
                    return NotFound();

                return Ok(@group);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error, the server is not responding");
            }
        }

        /// <summary>
        /// GET: api/Groups/search/5
        /// </summary>
        /// <param name="search">Параметр для поиска и фильтрации данных</param>
        /// <returns>Результат задачи содержит <see cref="List{T}"/> содержащий отфильтрованные элементы последовательности <see cref="Group"/></returns>
        /// <exception cref="Exception"></exception>
        [HttpGet("search/{search}")]
        public async Task<ActionResult<Group>> SearchGroup(string search)
        {
            try
            {
                if (_context.Groups is null)
                    return NotFound();

                var @group = await _context.Groups.Where(p => p.Name.ToLower().Trim().Contains(search.ToLower().Trim()) ||
                    p.Course.Name.ToLower().Trim().Contains(search.ToLower().Trim()) ||
                    p.Course.Teacher.FirstName.ToLower().Trim().Contains(search.ToLower().Trim()) ||
                    p.Course.Teacher.LastName.ToLower().Trim().Contains(search.ToLower().Trim()) ||
                    p.Course.Teacher.Patronymic!.ToLower().Trim().Contains(search.ToLower().Trim())).ToListAsync();

                return Ok(@group);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error, the server is not responding");
            }
        }

        /// <summary>
        /// PUT: api/Groups/5
        /// </summary>
        /// <param name="id">Параметр индификатора группы</param>
        /// <param name="group">Параметр обьекта <see cref="Group"/></param>
        /// <returns>Результат задачи, изменение обьекта класса <see cref="Group"/></returns>
        /// <exception cref="DbUpdateConcurrencyException"></exception>
        /// <exception cref="Exception"></exception>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroup(int id, Group @group)
        {
            try
            {
                if (id != @group.Id)
                    return BadRequest();

                _context.Entry(@group).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GroupExists(id))
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
        /// POST: api/Groups
        /// </summary>
        /// <param name="group">Параметр обьекта <see cref="Group"/></param>
        /// <returns>Результат задачи, новый обьект класса <see cref="Group"/></returns>
        /// <exception cref="Exception"></exception>
        [HttpPost]
        public async Task<ActionResult<Group>> PostGroup(Group @group)
        {
            try
            {
                var _course = await _context.Courses.FindAsync(group.CourseId);
                if (_course is null)
                    return BadRequest();

                await _context.AddAsync(new Group()
                {
                    Id = @group.Id,
                    Name = @group.Name,
                    Course = _course,
                    IsActive = @group.IsActive,
                });

                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(PostGroup), new { id = @group.Id }, @group);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error, the server is not responding");
            }
        }

        /// <summary>
        /// DELETE: api/Groups/5
        /// </summary>
        /// <param name="id">Параметр индификатора группы</param>
        /// <returns>Результат задачи, удаление обьекта класса <see cref="Group"/></returns>
        /// <exception cref="Exception"></exception>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGroup(int id)
        {
            try
            {
                if (_context.Groups is null)
                    return NotFound();

                var @group = await _context.Groups.FindAsync(id);
                if (@group is null)
                    return NotFound();

                _context.Groups.Remove(@group);
                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error, the server is not responding");
            }
        }

        private bool GroupExists(int id)
        {
            return (_context.Groups?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
