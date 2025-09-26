using EMPLOYEE.MANAGEMENT.CORE.models;
using EMPLOYEE.MANAGEMENT.REPOSITORY.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

namespace Employee_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] // Admin-only controller
    public class AdminController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly ApprovalCodeRepository _approvalCodeRepository;

        public AdminController(UserRepository userRepository, ApprovalCodeRepository approvalCodeRepository)
        {
            _userRepository = userRepository;
            _approvalCodeRepository = approvalCodeRepository;
        }

        // Create a user with a specified role (Admin-only)
        [HttpPost("users")] 
        public async Task<IActionResult> CreateUser([FromBody] UserData user)
        {
            if (user == null || string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password))
            {
                return BadRequest("Username and Password are required.");
            }

            var allowedRoles = new HashSet<string>(System.StringComparer.OrdinalIgnoreCase)
            {
                "Admin",
                "Moderator",
                "ReadOnly"
            };
            if (string.IsNullOrWhiteSpace(user.Role) || !allowedRoles.Contains(user.Role))
            {
                return BadRequest("Invalid role. Allowed roles: Admin, Moderator, ReadOnly.");
            }

            var success = await _userRepository.RegisterUserAsync(user);
            if (!success) return Conflict("Username already exists");
            return Ok("User created successfully");
        }

        // Promote or change a user's role (Admin-only)
        [HttpPut("users/{username}/role")]
        public async Task<IActionResult> UpdateUserRole(string username, [FromBody] string role)
        {
            var allowedRoles = new HashSet<string>(System.StringComparer.OrdinalIgnoreCase)
            {
                "Admin",
                "Moderator",
                "ReadOnly"
            };
            if (string.IsNullOrWhiteSpace(role) || !allowedRoles.Contains(role))
            {
                return BadRequest("Invalid role. Allowed roles: Admin, Moderator, ReadOnly.");
            }

            var updated = await _userRepository.UpdateUserRoleAsync(username, role);
            if (!updated) return NotFound("User not found or role unchanged");
            return NoContent();
        }

        // Create a short-lived approval code for Moderator registration
        [HttpPost("invites/moderator")]
        public async Task<ActionResult<object>> CreateModeratorApprovalCode([FromQuery] int expiresInHours = 24)
        {
            if (expiresInHours <= 0 || expiresInHours > 168) // up to 7 days
            {
                return BadRequest("expiresInHours must be between 1 and 168");
            }
            var code = GenerateNumericCode(8);
            var id = await _approvalCodeRepository.CreateAsync("Moderator", DateTime.UtcNow.AddHours(expiresInHours), User.Identity?.Name ?? "admin", code);
            return Ok(new { code, id }); // return raw code once to admin
        }

        private static string GenerateNumericCode(int length)
        {
            var random = new Random();
            var chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = (char)('0' + random.Next(0, 10));
            }
            return new string(chars);
        }
    }
}


