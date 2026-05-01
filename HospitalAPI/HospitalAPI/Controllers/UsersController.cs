using Microsoft.AspNetCore.Mvc;
using HospitalAPI.Data;
using HospitalAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace HospitalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly HospitalContext _context;

        public UsersController(HospitalContext context)
        {
            _context = context;
        }

        // ✅ REGISTER (1 user only)
        [HttpPost("register")]
        public async Task<IActionResult> Register(User user)
        {
            var existing = await _context.Users.FirstOrDefaultAsync();

            if (existing != null)
                return BadRequest("User already exists");

            user.CreatedAt = DateTime.Now;

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok("User created");
        }

        // ✅ LOGIN
        [HttpPost("login")]
        public async Task<IActionResult> Login(User login)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u =>
                u.Username == login.Username &&
                u.Password == login.Password);

            if (user == null)
                return Unauthorized("Invalid username or password");

            return Ok("Login successful");
        }
    }
}