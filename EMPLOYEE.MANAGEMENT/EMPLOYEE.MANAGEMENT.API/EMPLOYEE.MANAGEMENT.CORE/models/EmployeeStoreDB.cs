
namespace EMPLOYEE.MANAGEMENT.CORE.models;


/// <summary>
/// Strongly-typed configuration settings for the employee MongoDB store.
/// </summary>
public class EmployeeStoreDB : IEmployeeStoreDB
{
    /// <summary>
    /// Gets or sets the MongoDB collection name where employees are stored.
    /// </summary>
    public string EmployeesCollectionName { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the MongoDB connection string.
    /// </summary>
    public string ConnectionString { get; set; } = string.Empty;
    /// <summary>
    /// Gets or sets the MongoDB database name.
    /// </summary>
    public string DatabaseName { get; set; } = string.Empty;
}

