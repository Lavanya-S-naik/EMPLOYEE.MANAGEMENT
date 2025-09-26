using EMPLOYEE.MANAGEMENT.CORE.models;

namespace EMPLOYEE.MANAGEMENT.SERVICES.Service
{
    /// <summary>
    /// JWT service interface for token operations.
    /// </summary>
    public interface IJwtService
    {
        /// <summary>
        /// Generates a JWT token for the specified user.
        /// </summary>
        /// <param name="username">Username to generate token for.</param>
        /// <returns>JWT token string.</returns>
        string GenerateToken(string username, string userRole);

        /// <summary>
        /// Validates a JWT token and extracts user information.
        /// </summary>
        /// <param name="token">JWT token to validate.</param>
        /// <returns>Username if token is valid; otherwise null.</returns>
        string? ValidateToken(string token);
    }
}
