using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalAPI.Models
{
    public class Appointment
    {
        public int Id { get; set; }

        [Column("patient_id")]
        public int PatientId { get; set; }

        [Column("doctor_id")]
        public int DoctorId { get; set; }

        [Column("appointment_date")]
        public DateTime AppointmentDate { get; set; }

        [Column("appointment_time")]
        public TimeSpan AppointmentTime { get; set; }

        public string Reason { get; set; }
        public string Status { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }

        // navigation
        public Patient Patient { get; set; }
        public Doctor Doctor { get; set; }

        public string? Prescription { get; set; }
    }
}