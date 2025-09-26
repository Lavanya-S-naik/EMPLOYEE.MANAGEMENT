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

            var token = _jwtService.GenerateToken(user.Username, user.Role);
            var expiresAt = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes);
            _logger.LogInformation("Authentication successful for user: {Username}", loginRequest.Username);
            return new LoginResponse
            {
                Token = token,
                ExpiresAt = expiresAt,
                Username = user.Username
            };
        }
    }
}
