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
    public class GroupsController : ControllerBase
    {
        private readonly SmartSchoolboyBaseContext _context;

        public GroupsController(SmartSchoolboyBaseContext context)
        {
            _context = context;
        }

        // GET: api/Groups
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Group>>> GetGroups()
        {
          if (_context.Groups is null)
              return NotFound();

            return await _context.Groups.ToListAsync();
        }

        // GET: api/Groups/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Group>> GetGroup(int id)
        {
          if (_context.Groups is null)
              return NotFound();

            var @group = await _context.Groups.FindAsync(id);

            if (@group is null)
                return NotFound();

            return @group;
        }

        // PUT: api/Groups/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGroup(int id, Group @group)
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

        // POST: api/Groups
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
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

                return CreatedAtAction("GetGroup", new { id = @group.Id }, @group);
            }
            catch
            {
                return BadRequest();
            }
        }

        // DELETE: api/Groups/5
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
            catch
            {
                return BadRequest();
            }
        }

        private bool GroupExists(int id)
        {
            return (_context.Groups?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
