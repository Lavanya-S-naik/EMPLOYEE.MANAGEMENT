using EMPLOYEE.MANAGEMENT.CORE.models;
using EMPLOYEE.MANAGEMENT.REPOSITORY.Repository; // Add this using
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace EMPLOYEE.MANAGEMENT.SERVICES.Service
{
    public class AuthService : IAuthService
    {
        private readonly JwtSettings _jwtSettings;
        private readonly IJwtService _jwtService;
        private readonly ILogger<AuthService> _logger;
        private readonly UserRepository _userRepository; // Add this field

        public AuthService(
            IOptions<JwtSettings> jwtSettings,
            IJwtService jwtService,
            ILogger<AuthService> logger,
            UserRepository userRepository) // Add UserRepository to constructor
        {
            _jwtSettings = jwtSettings.Value;
            _jwtService = jwtService;
            _logger = logger;
            _userRepository = userRepository;
        }

        public async Task<LoginResponse?> AuthenticateAsync(LoginRequest loginRequest)
        {
            _logger.LogInformation("Authentication attempt for user: {Username}", loginRequest.Username);

            // Query MongoDB for the user
            var user = await _userRepository.GetByUsernameAsync(loginRequest.Username);
            if (user == null || user.Password != loginRequest.Password)
            {
                _logger.LogWarning("Authentication failed for user: {Username}", loginRequest.Username);
                return null;
            }

            // Handle both old single role and new multiple roles
            var userRoles = new List<string>();
            
            // If user has the new roles format, use it
            if (user.Roles != null && user.Roles.Any())
            {
                userRoles = user.Roles;
            }
            // If user has the old single role format, convert it
            else if (!string.IsNullOrEmpty(user.Role))
            {
                userRoles = new List<string> { user.Role };
            }
            
            var token = _jwtService.GenerateToken(user.Username, string.Join(",", userRoles));
            var expiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes);
            _logger.LogInformation("Authentication successful for user: {Username} with roles: {Roles}", loginRequest.Username, string.Join(",", userRoles));
            return new LoginResponse
            {
                Token = token,
                ExpiresAt = expiresAt,
                Username = user.Username,
                Roles = userRoles
            };
        }
    }
}
