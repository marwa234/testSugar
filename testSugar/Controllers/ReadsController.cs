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
    public class ReadsController : ControllerBase
    {
        private readonly SugarDbContext _context;

        public ReadsController(SugarDbContext context)
        {
            _context = context;
        }

        [HttpGet("average/{patientId}/{start}/{end}")]
        public async Task<decimal> Getaverage(decimal patientId, DateTime start, DateTime end)
        {

            var Reads = await _context.Reads.Where(r => r.Date >= start && r.Date <= end && r.PatientId == patientId).ToListAsync();
            decimal sum = 0;
            var count = Reads.Count();
            foreach (var R in Reads)
            {
                sum += R.Value;
            }
            var average = sum / count;

            return average;
        }

        [HttpGet("reads/{patientId}")]
        public async Task<ActionResult<IEnumerable<Read>>> GetReads(decimal patientId)
        {

            var Reads = await _context.Reads.Where(r => r.PatientId == patientId).ToListAsync();

            return Reads;
        }

        [HttpPost("{patientid}")]
        public async Task<int> PostRead(decimal patientid, Read read)
        {

            read.PatientId = patientid;
            _context.Reads.Add(read);
            await _context.SaveChangesAsync();

            decimal R = read.Value;

            var doses = await _context.Doses.Where(d => d.PatientId == patientid).ToListAsync();
            var dose = 0;

            foreach (var d in doses)
            {
                if ((d.R1 < R && d.R2 > R) || (d.R1 == R || d.R2 == R))
                {
                    dose = d.Dose1;
                    break;
                }
            }

            //if (R < doses[0].R1)
            //{
            //    dose = -1;
            //}
            if (R > doses[doses.Count - 1].R2)
            {
                dose = -2;
            }

            return dose;
        }

    }
}
