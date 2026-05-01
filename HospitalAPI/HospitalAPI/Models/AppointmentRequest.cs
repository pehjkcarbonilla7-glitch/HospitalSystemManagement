using System;

namespace HospitalAPI.Models
{
    public class AppointmentRequest
    {
        public string FirstName { get; set; }
        public string? MiddleName { get; set; }
        public string LastName { get; set; }

        public DateTime DateOfBirth { get; set; }
        public string? Gender { get; set; }

        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }

        public int DoctorId { get; set; }

        public DateTime AppointmentDate { get; set; }

        // 🔥 STRING first → convert later
        public string AppointmentTime { get; set; }

        public string? Reason { get; set; }
    }
}