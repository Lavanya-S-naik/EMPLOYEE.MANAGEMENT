namespace EMPLOYEE.MANAGEMENT.CORE.models
{
    /// <summary>
    /// Login response model.
    /// </summary>
    public class LoginResponse
    {
        public string Token { get; set; } = string.Empty;
        public DateTime ExpiresAt { get; set; }
        public string Username { get; set; } = string.Empty;
        public List<string> Roles { get; set; } = new List<string>();
    }
}
