using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchoolboyApi.Models;

namespace SmartSchoolboyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GendersController : ControllerBase
    {
        private readonly SmartSchoolboyBaseContext _context;

        public GendersController(SmartSchoolboyBaseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: api/Genders
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Gender>>> GetGenders()
        {
            try
            {
                if (_context.Genders is null)
                    return NotFound();

                return await _context.Genders.ToListAsync();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error, the server is not responding");
            }
        }
    }
}
