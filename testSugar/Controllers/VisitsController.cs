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
    public class VisitsController : ControllerBase
    {
        private readonly SugarDbContext _context;

        public VisitsController(SugarDbContext context)
        {
            _context = context;
        }

        [HttpGet("{patientID}")]
        public async Task<int> GetVisits(decimal patientID)
        {
            var patient_visits = await _context.Visits.Where(v => v.PatientId == patientID).ToListAsync();

            return patient_visits.Count;
        }

        [HttpGet("{patientID}/{num}")]
        public async Task<ActionResult<Visit>> GetVisit(decimal patientID, decimal num)
        {
            if (_context.Visits == null)
            {
                return NotFound();
            }
            var visit = await _context.Visits.FirstOrDefaultAsync(v => v.PatientId == patientID && v.VisitNo == num);

            if (visit == null)
            {
                return NotFound();
            }

            return visit;
        }

        [HttpPost("{patientID}")]
        public async Task<ActionResult<Visit>> PostVisit(decimal patientID, Visit visit)
        {
            if (_context.Visits == null)
            {
                return Problem("Entity set 'SugarDbContext.Visits'  is null.");
            }

            var patient_visits = await _context.Visits.Where(v => v.PatientId == patientID).ToListAsync();
            if (patient_visits.Count > 0)
            {
                visit.VisitNo = patient_visits.Count + 1;
            }
            else
            {
                visit.VisitNo = 1;
            }
            visit.PatientId = patientID;

            _context.Visits.Add(visit);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (VisitExists(visit.PatientId))
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


        private bool VisitExists(decimal id)
        {
            return (_context.Visits?.Any(e => e.PatientId == id)).GetValueOrDefault();
        }
    }
}
