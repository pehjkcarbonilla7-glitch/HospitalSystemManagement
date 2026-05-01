using Microsoft.EntityFrameworkCore;
using HospitalAPI.Models;

namespace HospitalAPI.Data
{
    public class HospitalContext : DbContext
    {
        public HospitalContext(DbContextOptions<HospitalContext> options)
            : base(options)
        {
        }

        public DbSet<Patient> Patients { get; set; }

        // ✅ ADD THIS
        public DbSet<Appointment> Appointments { get; set; }

        public DbSet<Doctor> Doctors { get; set; }

        public DbSet<User> Users { get; set; }
    }
}