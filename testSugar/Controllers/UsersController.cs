using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using testSuger.Models;

namespace testSuger.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly SugarDbContext _context;

        public UsersController(SugarDbContext context)
        {
            _context = context;
        }


        [HttpGet("{id}/{pass}/{role}")]
        public async Task<ActionResult<User>> GetUser(decimal id, string pass, string role)
        {
            if (_context.Users == null)
            {
                return NotFound();
            }
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == id && u.Password == pass && u.Role == role);

            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }


    }
}
