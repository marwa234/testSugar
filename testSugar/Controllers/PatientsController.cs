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
    [Consumes("application/json")]
    [Produces("application/json")]
    public class PatientsController : ControllerBase
    {
        private readonly SugarDbContext _context;

        public PatientsController(SugarDbContext context)
        {
            _context = context;
        }

        [HttpGet("allPatients/{doctorID}")]
        public async Task<ActionResult<IEnumerable<object>>> GetPatients(decimal doctorID)
        {
            if (_context.Patients == null)
            {
                return NotFound();
            }
            var patients_with_doctors = await _context.PatientDoctors.Where(pd => pd.DoctorId == doctorID).ToListAsync();

            var patients = new List<object>();
            foreach (var record in patients_with_doctors)
            {
                patients.Add(await _context.Patients.Select(p => new { p.Id, p.Name }).FirstOrDefaultAsync(pd => pd.Id == record.PatientId));
            }

            return patients;
        }


        [HttpGet("onePatient/{patientId}")]
        public async Task<ActionResult<Patient>> GetPatientByDoctor(decimal patientId)
        {
            if (_context.Patients == null)
            {
                return NotFound();
            }
            var patient = await _context.Patients.Include(d => d.Doses)
                                                 .Include(v => v.Visits)
                                                 .Include(r => r.Reads).FirstOrDefaultAsync(p => p.Id == patientId);
            int age = 0;
            //age = DateTime.Now.Subtract((DateTime)patient.Dob).Days;
            //age = age / 365;
            age = DateTime.Now.Year - patient.Dob.Year;
            if (DateTime.Now.DayOfYear < patient.Dob.DayOfYear)
                age = age - 1;

            patient.Age = age;

            //var doses = await _context.Doses.Where(d => d.PatientId == patientId).ToListAsync();
            //var visits = await _context.Visits.Where(d => d.PatientId == patientId).ToListAsync();

            if (patient == null)
            {
                return NotFound();
            }

            return patient; //Ok(new { patient, doses, visits })
        }


        [HttpGet("PatientNotify/{patientId}")]
        public async Task<bool> GetPatientNotify(decimal patientId)
        {
            var patient_time = await _context.Times.FirstOrDefaultAsync(p => p.PatientId == patientId);

            var h = DateTime.Now.TimeOfDay.Hours;
            var m = DateTime.Now.TimeOfDay.Minutes;
            var time_now = new TimeSpan(h, m, 00);

            var BM0 = patient_time.Breakfast;
            var BM5 = patient_time.Breakfast.Add(new TimeSpan(00, -5, 00));
            var BM10 = patient_time.Breakfast.Add(new TimeSpan(00, -10, 00));
            var BM15 = patient_time.Breakfast.Add(new TimeSpan(00, -15, 00));

            if (BM0 == time_now || BM15 == time_now || BM10 == time_now || BM5 == time_now)
            {
                return true;
            }

            var LM0 = patient_time.Lunch;
            var LM5 = patient_time.Lunch.Add(new TimeSpan(00, -5, 00));
            var LM10 = patient_time.Lunch.Add(new TimeSpan(00, -10, 00));
            var LM15 = patient_time.Lunch.Add(new TimeSpan(00, -15, 00));

            if (LM0 == time_now || LM15 == time_now || LM10 == time_now || LM5 == time_now)
            {
                return true;
            }

            var DM0 = patient_time.Dinner;
            var DM5 = patient_time.Dinner.Add(new TimeSpan(00, -5, 00));
            var DM10 = patient_time.Dinner.Add(new TimeSpan(00, -10, 00));
            var DM15 = patient_time.Dinner.Add(new TimeSpan(00, -15, 00));

            if (DM0 == time_now || DM15 == time_now || DM10 == time_now || DM5 == time_now)
            {
                return true;
            }

            if (patient_time == null)
            {
                return false;
            }

            return false;
        }


        [HttpPut("putDoctor/{patientID}/{doctorID}")]
        public async Task<IActionResult> PutDoctortoPatient(decimal patientID, decimal doctorID)
        {
            var patient = await _context.PatientDoctors.FirstOrDefaultAsync(p => p.PatientId == patientID);
            patient.DoctorId = doctorID;
            _context.Entry(patient).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("putPatient/{id}")]
        public async Task<IActionResult> PutPatient(decimal id, Patient patient)
        {
            if (id != patient.Id)
            {
                return BadRequest();
            }
            int age = 0;
            //age = DateTime.Now.Subtract((DateTime)patient.Dob).Days;
            //age = age / 365;
            age = DateTime.Now.Year - patient.Dob.Year;
            if (DateTime.Now.DayOfYear < patient.Dob.DayOfYear)
                age = age - 1;

            patient.Age = age;

            _context.Entry(patient).State = EntityState.Modified;

            //var v = 0;
            //if (!VisitExists(patient.Id))
            //{
            //    v = 1;
            //    _context.Visits.Add(
            //    new Visit()
            //    {
            //        VisitId = 1,
            //        PatientId = id,
            //    }
            //);
            //}
            //else
            //{
            //    v = _context.Visits.Where(v => v.PatientId == patient.Id).Count();
            //    _context.Visits.Add(
            //    new Visit()
            //    {
            //        VisitId = v + 1,
            //        PatientId = id,
            //    }
            //    );
            //}


            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatientExists(id))
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

        [HttpPost("{doctorID}")]
        public async Task<ActionResult<Patient>> PostPatient(decimal doctorID, Patient patient)
        {
            try
            {
                if (_context.Patients == null)
                {
                    return Problem("Entity set 'SugarDbContext.Patients'  is null.");
                }

                int age = 0;
                //age = DateTime.Now.Subtract((DateTime)patient.Dob).Days;
                //age = age / 365;
                age = DateTime.Now.Year - patient.Dob.Year;
                if (DateTime.Now.DayOfYear < patient.Dob.DayOfYear)
                    age = age - 1;

                patient.Age = age;

                _context.Users.Add(
                    new User()
                    {
                        Id = patient.Id,
                        Password = "123456789",
                        Role = "P"
                    }
                );
                await _context.SaveChangesAsync();

                _context.Patients.Add(patient);

                _context.PatientDoctors.Add(
                    new PatientDoctor()
                    {
                        PatientId = patient.Id,
                        DoctorId = doctorID,
                    }
                );

                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PatientExists(patient.Id) || DoctorExists(doctorID))
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
        public async Task<IActionResult> DeletePatient(decimal id)
        {
            if (_context.Patients == null)
            {
                return NotFound();
            }
            var patient = await _context.Patients.FindAsync(id);
            if (patient == null)
            {
                return NotFound();
            }

            _context.Patients.Remove(patient);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PatientExists(decimal id)
        {
            return (_context.Patients?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private bool DoctorExists(decimal id)
        {
            return (_context.Users?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        private bool VisitExists(decimal id)
        {
            return (_context.Visits?.Any(e => e.PatientId == id)).GetValueOrDefault();
        }
    }
}
