using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using HospitalAPI.Data;
using HospitalAPI.Models;

namespace HospitalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DoctorsController : ControllerBase
    {
        private readonly HospitalContext _context;

        public DoctorsController(HospitalContext context)
        {
            _context = context;
        }

        // ✅ GET ALL
        [HttpGet]
        public async Task<IActionResult> GetDoctors()
        {
            var doctors = await _context.Doctors.ToListAsync();
            return Ok(doctors);
        }

        // ✅ GET BY ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetDoctor(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);

            if (doctor == null)
                return NotFound();

            return Ok(doctor);
        }

        // ✅ ADD DOCTOR
        [HttpPost]
        public async Task<IActionResult> AddDoctor(Doctor doctor)
        {
            doctor.DateHired = DateTime.Now; // auto set

            _context.Doctors.Add(doctor);
            await _context.SaveChangesAsync();

            return Ok(doctor);
        }

        // ✅ UPDATE
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDoctor(int id, Doctor updated)
        {
            var doctor = await _context.Doctors.FindAsync(id);

            if (doctor == null)
                return NotFound();

            doctor.FirstName = updated.FirstName;
            doctor.LastName = updated.LastName;
            doctor.Specialization = updated.Specialization;
            doctor.Department = updated.Department;
            doctor.Phone = updated.Phone;
            doctor.Email = updated.Email;

            await _context.SaveChangesAsync();

            return Ok("Doctor updated successfully");
        }

        // ✅ DELETE
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDoctor(int id)
        {
            var doctor = await _context.Doctors.FindAsync(id);

            if (doctor == null)
                return NotFound();

            _context.Doctors.Remove(doctor);
            await _context.SaveChangesAsync();

            return Ok("Doctor deleted successfully");
        }
    }
}