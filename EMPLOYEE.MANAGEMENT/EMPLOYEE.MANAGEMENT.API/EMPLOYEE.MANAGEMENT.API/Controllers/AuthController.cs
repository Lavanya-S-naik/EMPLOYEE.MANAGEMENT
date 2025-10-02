using EMPLOYEE.MANAGEMENT.CORE.models;
using EMPLOYEE.MANAGEMENT.REPOSITORY.Repository;
using EMPLOYEE.MANAGEMENT.SERVICES.Service;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace Employee_Management.Controllers
{
    /// <summary>
    /// Authentication controller for JWT token generation.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly ILogger<AuthController> _logger;
        private readonly UserRepository _userRepository;

        public AuthController(IAuthService authService, ILogger<AuthController> logger, UserRepository userRepository)
        {
            _authService = authService;
            _logger = logger;
            _userRepository = userRepository;
        }

        /// <summary>
        /// Registers a new user with specified role.
        /// </summary>
        /// <param name="request">Registration details</param>
        /// <returns>Success message if registration successful</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            _logger.LogInformation("Registration attempt for user: {Username}", request.Username);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid registration request model");
                return ValidationProblem(ModelState);
            }

            // Only allow ReadOnly for public registration
            var allowedRoles = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "ReadOnly"
            };

            if (request?.Roles == null || !request.Roles.Any())
            {
                _logger.LogWarning("No roles provided for registration");
                return BadRequest("At least one role is required.");
            }

            // Validate all provided roles - only ReadOnly allowed for public registration
            var invalidRoles = request.Roles.Where(role => !allowedRoles.Contains(role)).ToList();
            if (invalidRoles.Any())
            {
                _logger.LogWarning("Invalid roles provided for public registration: {InvalidRoles}", string.Join(", ", invalidRoles));
                return BadRequest($"Invalid roles: {string.Join(", ", invalidRoles)}. Public registration only allows: ReadOnly. For Moderator/Admin roles, contact an administrator.");
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
                _logger.LogWarning("Registration failed - username already exists: {Username}", request.Username);
                return BadRequest("Username already exists");
            }

            _logger.LogInformation("Registration successful for user: {Username} with roles: {Roles}", request.Username, string.Join(",", request.Roles));
            return Ok("Registration successful");
        }

        /// <summary>
        /// Adds a new role to an existing user.
        /// </summary>
        /// <param name="username">Username to add role to</param>
        /// <param name="role">Role to add</param>
        /// <returns>Success message if role added successfully</returns>
        //[HttpPost("add-role/{username}")]
        //public async Task<IActionResult> AddRoleToUser(string username, [FromBody] string role)
        //{
        //    _logger.LogInformation("Adding role {Role} to user {Username}", role, username);

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

        /// <summary>
        /// Registers a new Moderator with approval code.
        /// </summary>
        /// <param name="request">Moderator registration with approval code</param>
        /// <returns>Success message if registration successful</returns>
        /// 






        [HttpPost("register-moderator")]
        public async Task<IActionResult> RegisterModerator([FromBody] ModeratorRegistrationRequest request)
        {
            // 1. Find the user
            var user = await _userRepository.GetByUsernameAsync(request.Username);
            if (user == null) return BadRequest("User not found");

            // 2. Validate password (for security)
            if (user.Password != request.Password) return Unauthorized("Invalid password");

            // 3. Validate approval code (must match the code generated for this user)
            if (!AdminController.approvalCodes.TryGetValue(request.Username, out var storedCode) ||
                storedCode != request.ApprovalCode)
                return BadRequest("Invalid approval code");

            // 4. Add 'Moderator' role (if not already present)
            if (!user.Roles.Contains("Moderator"))
            {
                user.Roles.Add("Moderator");
                await _userRepository.UpdateUserAsync(user); // Implement this!
                                                            // Remove/expire the code after use
                AdminController.approvalCodes.Remove(request.Username);
                return Ok("Added Moderator role. User now has: " + string.Join(", ", user.Roles));
            }
            else
            {
                return Ok("User already has Moderator. Roles: " + string.Join(", ", user.Roles));
            }
        }






        //[HttpPost("register-moderator")]
        //public async Task<IActionResult> RegisterModerator([FromBody] ModeratorRegistrationRequest request)
        //{
        //    _logger.LogInformation("Moderator registration attempt for user: {Username}", request.Username);

        //    if (!ModelState.IsValid)
        //    {
        //        _logger.LogWarning("Invalid moderator registration request model");
        //        return ValidationProblem(ModelState);
        //    }

        //    if (string.IsNullOrWhiteSpace(request.ApprovalCode))
        //    {
        //        _logger.LogWarning("Approval code required for Moderator registration");
        //        return BadRequest("Approval code is required for Moderator registration.");
        //    }

        //    // Validate approval code (you'll need to implement this)
        //    // For now, we'll accept any non-empty approval code
        //    if (request.ApprovalCode.Length < 6)
        //    {
        //        _logger.LogWarning("Invalid approval code format");
        //        return BadRequest("Invalid approval code format.");
        //    }

        //    var user = new UserData
        //    {
        //        Username = request.Username,
        //        Password = request.Password,
        //        Roles = new List<string> { "Moderator" }
        //    };

        //    var success = await _userRepository.RegisterUserAsync(user);
        //    if (!success)
        //    {
        //        _logger.LogWarning("Moderator registration failed - username already exists: {Username}", request.Username);
        //        return BadRequest("Username already exists");
        //    }

        //    _logger.LogInformation("Moderator registration successful for user: {Username}", request.Username);
        //    return Ok("Moderator registration successful");
        //}





        /// <summary>
        /// Authenticates a user and returns a JWT token.
        /// </summary>
        /// <param name="loginRequest">Login credentials</param>
        /// <returns>JWT token if authentication successful</returns>
        [HttpPost("login")]
        public async Task<ActionResult<LoginResponse>> Login([FromBody] LoginRequest loginRequest)
        {
            _logger.LogInformation("Login attempt for user: {Username}", loginRequest.Username);

            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid login request model");
                return ValidationProblem(ModelState);
            }

            var result = await _authService.AuthenticateAsync(loginRequest);
            
            if (result == null)
            {
                _logger.LogWarning("Login failed for user: {Username}", loginRequest.Username);
                return Unauthorized(new { message = "Invalid username or password" });
            }

            _logger.LogInformation("Login successful for user: {Username}", loginRequest.Username);
            return Ok(result);
        }




    }
}
