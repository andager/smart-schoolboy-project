using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SmartSchoolboyApi.Models;

namespace SmartSchoolboyApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly SmartSchoolboyBaseContext _context;

        public RolesController(SmartSchoolboyBaseContext context)
        {
            _context = context;
        }

        /// <summary>
        /// GET: api/Roles
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> GetRoles()
        {
            try
            {
                if (_context.Roles is null)
                    return NotFound();

                return await _context.Roles.ToListAsync();

            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Server error, the server is not responding");
            }
        }
    }
}
