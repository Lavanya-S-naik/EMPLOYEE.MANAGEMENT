using EMPLOYEE.MANAGEMENT.CORE.models;
using EMPLOYEE.MANAGEMENT.REPOSITORY.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Employee_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Admin")] // Admin-only controller
    public class AdminController : ControllerBase
    {
        private readonly UserRepository _userRepository;
        private readonly ILogger<AdminController> _logger;

        public AdminController(UserRepository userRepository, ILogger<AdminController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        /// <summary>
        /// Creates a new user with specified role (Admin-only).
        /// </summary>
        /// <param name="request">User creation request</param>
        /// <returns>Success message if user created successfully</returns>
        [HttpPost("create-user")]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
        {
            _logger.LogInformation("Admin creating user: {Username} with roles: {Roles}", request.Username, string.Join(",", request.Roles));

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid create user request model");
                return ValidationProblem(ModelState);
            }

            var allowedRoles = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "Admin",
                "Moderator",
                "ReadOnly"
            };

            if (request?.Roles == null || !request.Roles.Any())
            {
                _logger.LogWarning("No roles provided for user creation");
                return BadRequest("At least one role is required.");
            }

            // Validate all provided roles
            var invalidRoles = request.Roles.Where(role => !allowedRoles.Contains(role)).ToList();
            if (invalidRoles.Any())
            {
                _logger.LogWarning("Invalid roles provided: {InvalidRoles}", string.Join(", ", invalidRoles));
                return BadRequest($"Invalid roles: {string.Join(", ", invalidRoles)}. Allowed roles: Admin, Moderator, ReadOnly.");
            }

            var user = new UserData
            {
                Username = request.Username,
                Password = request.Password,
                Roles = request.Roles
            };

            var success = await _userRepository.RegisterUserAsync(user);
            if (!success)
            {
                _logger.LogWarning("User creation failed - username already exists: {Username}", request.Username);
                return BadRequest("Username already exists");
            }

            _logger.LogInformation("User created successfully: {Username} with roles: {Roles}", request.Username, string.Join(",", request.Roles));
            return Ok($"User {request.Username} created successfully with roles: {string.Join(", ", request.Roles)}");
        }

        /// <summary>
        /// Generates an approval code for Moderator registration.
        /// </summary>
        /// <returns>Approval code for Moderator registration</returns>
        /// 




        //[HttpPost("generate-moderator-code")]
        //public async Task<IActionResult> GenerateModeratorCode()
        //{
        //    _logger.LogInformation("Generating moderator approval code");

        //    // Generate a random 8-digit approval code
        //    var random = new Random();
        //    var approvalCode = random.Next(10000000, 99999999).ToString();

        //    _logger.LogInformation("Moderator approval code generated: {Code}", approvalCode);
        //    return Ok(new { approvalCode = approvalCode, message = "Approval code generated successfully" });
        //}




        // Add an in-memory store for the example; for production, use a DB or persistent cache
        public static Dictionary<string, string> approvalCodes = new Dictionary<string, string>();

        [HttpPost("generate-moderator-code")]
        public IActionResult GenerateModeratorCode([FromBody] string username)
        {
            _logger.LogInformation("Generating moderator approval code");
            // Generate a random code
            var code = Guid.NewGuid().ToString().Substring(0, 8);
            approvalCodes[username] = code; // Map code to username

            return Ok(new { username, approvalCode = code, message = "Approval code generated successfully" });
         
        }







        /// <summary>
        /// Adds a role to an existing user (Admin-only).
        /// </summary>
        /// <param name="username">Username to add role to</param>
        /// <param name="role">Role to add</param>
        /// <returns>Success message if role added successfully</returns>
        /// 




        //[HttpPost("add-role/{username}")]
        //public async Task<IActionResult> AddRoleToUser(string username, [FromBody] string role)
        //{
        //    _logger.LogInformation("Admin adding role {Role} to user {Username}", role, username);

        //    if (string.IsNullOrWhiteSpace(role))
        //    {
        //        return BadRequest("Role is required.");
        //    }

        //    var allowedRoles = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        //    {
        //        "Admin",
        //        "Moderator",
        //        "ReadOnly"
        //    };

        //    if (!allowedRoles.Contains(role))
        //    {
        //        return BadRequest($"Invalid role. Allowed roles: Admin, Moderator, ReadOnly.");
        //    }

        //    var success = await _userRepository.AddRoleToUserAsync(username, role);
        //    if (!success)
        //    {
        //        _logger.LogWarning("Failed to add role {Role} to user {Username}", role, username);
        //        return BadRequest("User not found or role already exists");
        //    }

        //    _logger.LogInformation("Successfully added role {Role} to user {Username}", role, username);
        //    return Ok($"Role {role} added to user {username}");
        //}
    }
}
