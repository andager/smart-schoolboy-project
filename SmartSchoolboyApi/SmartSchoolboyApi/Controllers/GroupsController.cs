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
        /// <returns></returns>
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
        /// <param name="id"></param>
        /// <returns></returns>
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
        /// GET: api/Groups/search
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
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
                    p.Course.Teacher.Patronymic.ToLower().Trim().Contains(search.ToLower().Trim())).ToListAsync();

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
        /// <param name="id"></param>
        /// <param name="group"></param>
        /// <returns></returns>
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
        /// <param name="group"></param>
        /// <returns></returns>
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
        /// <param name="id"></param>
        /// <returns></returns>
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

                return NoContent();
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
