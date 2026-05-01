using Microsoft.AspNetCore.Mvc;
using HospitalAPI.Data;
using HospitalAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppointmentsController : ControllerBase
    {
        private readonly HospitalContext _context;

        public AppointmentsController(HospitalContext context)
        {
            _context = context;
        }

        // ✅ CREATE FULL (PATIENT + APPOINTMENT)
        [HttpPost("full")]
        public async Task<IActionResult> CreateFull(AppointmentRequest req)
        {
            if (string.IsNullOrEmpty(req.FirstName) || req.DoctorId == 0)
                return BadRequest("Incomplete data");

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // 🔥 SAVE PATIENT
                var patient = new Patient
                {
                    FirstName = req.FirstName,
                    MiddleName = req.MiddleName ?? "",
                    LastName = req.LastName,
                    DateOfBirth = req.DateOfBirth,
                    Gender = req.Gender ?? "",
                    Address = req.Address ?? "",
                    Phone = req.Phone ?? "",
                    Email = req.Email ?? "",
                    Age = DateTime.Now.Year - req.DateOfBirth.Year,
                    DateRegistered = DateTime.Now
                };

                _context.Patients.Add(patient);
                await _context.SaveChangesAsync();

                // 🔥 CONVERT TIME
                if (!TimeSpan.TryParse(req.AppointmentTime, out var time))
                    return BadRequest("Invalid time format. Use HH:mm:ss");

                // 🔥 SAVE APPOINTMENT
                var appointment = new Appointment
                {
                    PatientId = patient.Id,
                    DoctorId = req.DoctorId,
                    AppointmentDate = req.AppointmentDate,
                    AppointmentTime = time,
                    Reason = req.Reason ?? "",
                    Status = "Pending",
                    CreatedAt = DateTime.Now
                };

                _context.Appointments.Add(appointment);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();

                return Ok(new
                {
                    message = "Appointment Saved Successfully",
                    patientId = patient.Id
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }

        // ✅ GET ALL (WITH JOIN)
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var data = await _context.Appointments
                .Include(a => a.Patient)
                .Include(a => a.Doctor)
                .ToListAsync();

            return Ok(data);
        }

        // ✅ DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var appointment = await _context.Appointments.FindAsync(id);

            if (appointment == null)
                return NotFound();

            _context.Appointments.Remove(appointment);
            await _context.SaveChangesAsync();

            return Ok("Deleted successfully");
        }

        // ✅ UPDATE STATUS
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] string status)
        {
            var appointment = await _context.Appointments.FindAsync(id);

            if (appointment == null)
                return NotFound();

            appointment.Status = status;
            await _context.SaveChangesAsync();

            return Ok("Status updated");
        }
    }
}