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
    public class MedicinesController : ControllerBase
    {
        private readonly SugarDbContext _context;

        public MedicinesController(SugarDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Medicine>>> GetMedicines()
        {
            try
            {
                if (_context.Medicines == null)
                {
                    return NotFound();
                }
                return await _context.Medicines.ToListAsync();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        public async Task<ActionResult<Medicine>> PostMedicine(Medicine medicine)
        {
            if (_context.Medicines == null)
            {
                return Problem("Entity set 'SugarDbContext.Medicines'  is null.");
            }
            _context.Medicines.Add(medicine);
            await _context.SaveChangesAsync();

            return Ok();
        }

    }
}
