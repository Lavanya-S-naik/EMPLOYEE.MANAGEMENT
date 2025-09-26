using System.ComponentModel.DataAnnotations;

namespace EMPLOYEE.MANAGEMENT.CORE.models
{
    /// <summary>
    /// Login request model.
    /// </summary>
    public class LoginRequest
    {
        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
