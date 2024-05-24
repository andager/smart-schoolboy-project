using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchoolboyApi.Models;

namespace SmartSchoolboyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ControlThemePlanesController : ControllerBase
    {
        private readonly SmartSchoolboyBaseContext _context;

        public ControlThemePlanesController(SmartSchoolboyBaseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: api/ControlThemePlanes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ControlThemePlane>>> GetControlThemePlanes()
        {
            try
            {
                if (_context.ControlThemePlanes is null)
                    return NotFound();

                return Ok(await _context.ControlThemePlanes.ToListAsync());
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error, the server is not responding");
            }
        }

        /// <summary>
        /// GET: api/ControlThemePlanes/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ControlThemePlane>> GetControlThemePlane(int id)
        {
            try
            {
                if (_context.ControlThemePlanes is null)
                    return NotFound();

                var controlThemePlane = await _context.ControlThemePlanes.FindAsync(id);

                if (controlThemePlane is null)
                    return NotFound();

                return Ok(controlThemePlane);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error, the server is not responding");
            }
        }

        /// <summary>
        /// PUT: api/ControlThemePlanes/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="controlThemePlane"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> PutControlThemePlane(int id, ControlThemePlane controlThemePlane)
        {
            try
            {
                if (id != controlThemePlane.Id)
                    return BadRequest();

                _context.Entry(controlThemePlane).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ControlThemePlaneExists(id))
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
        /// POST: api/ControlThemePlanes
        /// </summary>
        /// <param name="controlThemePlane"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult<ControlThemePlane>> PostControlThemePlane(ControlThemePlane controlThemePlane)
        {
            try
            {
                await _context.AddAsync(new ControlThemePlane()
                {
                    Id = controlThemePlane.Id,
                    LessonName = controlThemePlane.LessonName,
                    LessonDescription = controlThemePlane.LessonDescription,
                });

                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(PostControlThemePlane), new { id = controlThemePlane.Id }, controlThemePlane);
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error, the server is not responding");
            }
        }

        /// <summary>
        /// DELETE: api/ControlThemePlanes/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteControlThemePlane(int id)
        {
            try
            {
                if (_context.ControlThemePlanes is null)
                    return NotFound();

                var controlThemePlane = await _context.ControlThemePlanes.FindAsync(id);
                if (controlThemePlane is null)
                    return NotFound();

                _context.ControlThemePlanes.Remove(controlThemePlane);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error, the server is not responding");
            }
        }

        private bool ControlThemePlaneExists(int id)
        {
            return (_context.ControlThemePlanes?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
