using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    public class TimesController : ControllerBase
    {
        private readonly SugarDbContext _context;

        public TimesController(SugarDbContext context)
        {
            _context = context;
        }

        [HttpGet("{patientid}")]
        public async Task<ActionResult<Time>> GetTime(decimal patientid)
        {
            if (_context.Times == null)
            {
                return NotFound();
            }
            var h = DateTime.Now.TimeOfDay.Hours;
            var m = DateTime.Now.TimeOfDay.Minutes;
            var time_now = new TimeSpan(h, m, 00);

            var time = await _context.Times.FindAsync(patientid);

            var B_before = time.Breakfast.Add(new TimeSpan(-1, 00, 00));
            var B_after = time.Breakfast.Add(new TimeSpan(1, 00, 00));

            //var timeDifference = time.Breakfast - time.Lunch;

            int token = 0;
            if (time_now >= B_before && time_now <= B_after)
            {
                token = 1;
            }

            var L_before = time.Lunch.Add(new TimeSpan(-1, 00, 00));
            var L_after = time.Lunch.Add(new TimeSpan(1, 00, 00));
            if (time_now >= L_before && time_now <= L_after)
            {
                token = 2;
            }

            var D_before = time.Dinner.Add(new TimeSpan(-1, 00, 00));
            var D_after = time.Dinner.Add(new TimeSpan(1, 00, 00));
            if (time_now >= D_before && time_now <= D_after)
            {
                token = 3;
            }


            if (time == null)
            {
                return NotFound();
            }

            return Ok(new { time, token });
        }

        [HttpPut("{patientid}")]
        public async Task<IActionResult> PutTime(decimal patientid, Time time)
        {
            if (patientid != time.PatientId)
            {
                return BadRequest();
            }

            _context.Entry(time).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TimeExists(patientid))
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

        [HttpPost("{patientid}")]
        public async Task<ActionResult<Time>> PostTime(decimal patientid, Time time)
        {
            if (_context.Times == null)
            {
                return Problem("Entity set 'SugarDbContext.Times'  is null.");
            }

            time.PatientId = patientid;
            _context.Times.Add(time);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TimeExists(time.PatientId))
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

        private bool TimeExists(decimal id)
        {
            return (_context.Times?.Any(e => e.PatientId == id)).GetValueOrDefault();
        }
    }
}
