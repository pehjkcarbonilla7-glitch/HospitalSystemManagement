using HospitalAPI.Models;
using HospitalAPI.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[Route("api/[controller]")]
[ApiController]
public class PatientController : ControllerBase
{
    private readonly HospitalContext _context;

    public PatientController(HospitalContext context)
    {
        _context = context;
    }

    // GET ALL
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Patient>>> GetPatients()
    {
        return await _context.Patients.ToListAsync();
    }

    // ADD
    [HttpPost]
    public async Task<ActionResult<Patient>> AddPatient(Patient patient)
    {
        // AUTO COMPUTE AGE
        patient.Age = DateTime.Now.Year - patient.DateOfBirth.Year;

        _context.Patients.Add(patient);
        await _context.SaveChangesAsync();

        return Ok(patient);
    }

    // UPDATE
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePatient(int id, Patient patient)
    {
        if (id != patient.Id)
            return BadRequest("ID mismatch");

        var existing = await _context.Patients.FindAsync(id);

        if (existing == null)
            return NotFound("Patient not found");

        // UPDATE VALUES
        existing.FirstName = patient.FirstName;
        existing.MiddleName = patient.MiddleName;
        existing.LastName = patient.LastName;
        existing.DateOfBirth = patient.DateOfBirth;
        existing.Age = DateTime.Now.Year - patient.DateOfBirth.Year;
        existing.Gender = patient.Gender;
        existing.CivilStatus = patient.CivilStatus;
        existing.Address = patient.Address;
        existing.Phone = patient.Phone;
        existing.Email = patient.Email;

        await _context.SaveChangesAsync();

        return Ok("Patient Updated Successfully");
    }

    // DELETE
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePatient(int id)
    {
        var patient = await _context.Patients.FindAsync(id);

        if (patient == null)
            return NotFound("Patient not found");

        _context.Patients.Remove(patient);
        await _context.SaveChangesAsync();

        return Ok("Patient Deleted Successfully");
    }
}