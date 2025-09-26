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
        private readonly IJwtService _jwtService;
        private readonly ILogger<AuthController> _logger;
        private readonly UserRepository _userRepository;




        public AuthController(IAuthService authService, IJwtService jwtService, ILogger<AuthController> logger, UserRepository userRepository)
        {
            _authService = authService;
            _jwtService = jwtService;
            _logger = logger;
            _userRepository = userRepository;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            // Enforce only allowed roles via public registration (no Admin)
            var allowedRoles = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                "Moderator",
                "ReadOnly"
            };

            if (string.IsNullOrWhiteSpace(request?.Role) || !allowedRoles.Contains(request.Role))
            {
                return BadRequest("Invalid role. Public registration allows only: Moderator, ReadOnly.");
            }

            // If registering as Moderator, require a valid approval code issued by Admin
            if (string.Equals(request.Role, "Moderator", StringComparison.OrdinalIgnoreCase))
            {
                if (string.IsNullOrWhiteSpace(request.ApprovalCode))
                {
                    return BadRequest("Approval code is required to register as Moderator.");
                }
                // Validate and consume the approval code
                var approvalRepo = HttpContext.RequestServices.GetService(typeof(EMPLOYEE.MANAGEMENT.REPOSITORY.Repository.ApprovalCodeRepository)) as EMPLOYEE.MANAGEMENT.REPOSITORY.Repository.ApprovalCodeRepository;
                if (approvalRepo == null)
                {
                    return StatusCode(500, "Approval system not available");
                }
                var ok = await approvalRepo.ValidateAndConsumeAsync(request.ApprovalCode, "Moderator");
                if (!ok)
                {
                    return BadRequest("Invalid or expired approval code.");
                }
            }

            var user = new UserData
            {
                Username = request.Username,
                Password = request.Password,
                Role = request.Role
            };
            var success = await _userRepository.RegisterUserAsync(user);
            if (!success) return BadRequest("Username already exists");
            return Ok("Registration successful");
        }


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

        /// <summary>
        /// Validates a JWT token and returns user information.
        /// </summary>
        /// <param name="token">JWT token to validate</param>
        /// <returns>User information if token is valid</returns>
        [HttpPost("validate")]
        public ActionResult<object> ValidateToken([FromBody] string token)
        {
            _logger.LogInformation("Token validation request received");

            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Empty token provided for validation");
                return BadRequest(new { message = "Token is required" });
            }

            var username = _jwtService.ValidateToken(token);
            
            if (username == null)
            {
                _logger.LogWarning("Token validation failed");
                return Unauthorized(new { message = "Invalid token" });
            }

            _logger.LogInformation("Token validation successful for user: {Username}", username);
            return Ok(new { username = username, valid = true });
        }



    }
}
