using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HospitalAPI.Models
{
    public class User
    {
        public int Id { get; set; }

        [Column("username")]
        public string Username { get; set; }

        [Column("password")]
        public string Password { get; set; }

        [Column("created_at")]
        public DateTime CreatedAt { get; set; }
    }
}