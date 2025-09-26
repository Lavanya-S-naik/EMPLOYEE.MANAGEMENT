using EMPLOYEE.MANAGEMENT.CORE.models;

namespace EMPLOYEE.MANAGEMENT.SERVICES.Service
{
    /// <summary>
    /// Simple authentication service interface.
    /// </summary>
    public interface IAuthService
    {
        Task<LoginResponse?> AuthenticateAsync(LoginRequest loginRequest);
    }
}
