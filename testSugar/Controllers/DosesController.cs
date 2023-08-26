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
    public class DosesController : ControllerBase
    {
        private readonly SugarDbContext _context;

        public DosesController(SugarDbContext context)
        {
            _context = context;
        }

        [HttpGet("{patientID}")]
        public async Task<ActionResult<IEnumerable<Dose>>> GetDoses(decimal patientID)
        {
            if (_context.Doses == null)
            {
                return NotFound();
            }
            var doses = await _context.Doses.Where(d => d.PatientId == patientID).ToListAsync();
            return doses;
        }

        [HttpGet("{patientId}/{read}")]
        public async Task<int> GetDose(int patientId, decimal read)
        {
            var doses = await _context.Doses.Where(d => d.PatientId == patientId).ToListAsync();
            var dose = 0;

            foreach (var d in doses)
            {
                if ((d.R1 < read && d.R2 > read) || (d.R1 == read || d.R2 == read))
                {
                    dose = d.Dose1;
                    break;
                }
            }

            if (read < doses[0].R1)
            {
                dose = -1;
            }
            else if (read > doses[doses.Count - 1].R2)
            {
                dose = -2;
            }

            return dose;
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Dose>> PutDose(int id, Dose dose)
        {
            if (id != dose.DoseId)
            {
                return BadRequest();
            }

            _context.Entry(dose).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DoseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        [HttpPost("{patientID}")]
        public async Task<ActionResult<Dose>> PostDose(decimal patientID, Dose dose)
        {
            if (_context.Doses == null)
            {
                return Problem("Entity set 'SugarDbContext.Doses'  is null.");
            }

            dose.PatientId = patientID;
            _context.Doses.Add(dose);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DoseExists(dose.DoseId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDose(int id)
        {
            if (_context.Doses == null)
            {
                return NotFound();
            }
            var dose = await _context.Doses.FindAsync(id);
            if (dose == null)
            {
                return NotFound();
            }

            _context.Doses.Remove(dose);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool DoseExists(int id)
        {
            return (_context.Doses?.Any(e => e.DoseId == id)).GetValueOrDefault();
        }
    }
}
