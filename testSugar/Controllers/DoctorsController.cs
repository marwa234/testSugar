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
    public class DoctorsController : ControllerBase
    {
        private readonly SugarDbContext _context;

        public DoctorsController(SugarDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Doctor>> PostDoctor(Doctor doctor)
        {
            try
            {
                if (_context.Doctors == null)
                {
                    return Problem("Entity set 'SugarDbContext.Doctors'  is null.");
                }
                _context.Users.Add(
                    new User()
                    {
                        Id = doctor.Id,
                        Password = "123456789",
                        Role = "D"
                    }
                );
                await _context.SaveChangesAsync();

                int age = 0;
                //age = DateTime.Now.Subtract((DateTime)patient.Dob).Days;
                //age = age / 365;
                age = DateTime.Now.Year - doctor.Dob.Year;
                if (DateTime.Now.DayOfYear < doctor.Dob.DayOfYear)
                    age = age - 1;

                doctor.Age = age;

                _context.Doctors.Add(doctor);

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DoctorExists(doctor.Id))
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
        public async Task<IActionResult> DeleteDoctor(decimal id)
        {
            if (_context.Doctors == null)
            {
                return NotFound();
            }
            var doctor = await _context.Doctors.FindAsync(id);
            if (doctor == null)
            {
                return NotFound();
            }

            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DoctorExists(decimal id)
        {
            return (_context.Doctors?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
