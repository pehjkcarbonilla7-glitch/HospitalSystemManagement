using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalAPI.Models
{
    public class Patient
    {
        public int Id { get; set; }

        [Column("first_name")]
        public string FirstName { get; set; }

        [Column("middle_name")]
        public string? MiddleName { get; set; }

        [Column("last_name")]
        public string LastName { get; set; }

        [Column("date_of_birth")]
        public DateTime DateOfBirth { get; set; }

        [Column("age")]
        public int Age { get; set; }

        [Column("gender")]
        public string? Gender { get; set; }

        [Column("civil_status")]
        public string? CivilStatus { get; set; }

        [Column("address")]
        public string? Address { get; set; }

        [Column("phone")]
        public string? Phone { get; set; }

        [Column("email")]
        public string? Email { get; set; }

        [Column("date_registered")]
        public DateTime DateRegistered { get; set; }
    }
}