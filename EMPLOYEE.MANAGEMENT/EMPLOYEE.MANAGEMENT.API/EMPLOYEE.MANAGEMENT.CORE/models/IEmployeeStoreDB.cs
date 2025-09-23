namespace EMPLOYEE.MANAGEMENT.CORE.models;

/// <summary>
/// Defines configuration settings required to access the employee data store.
/// </summary>
public interface IEmployeeStoreDB
{
    /// <summary>
    /// Gets or sets the MongoDB collection name for employees.
    /// </summary>
    string EmployeesCollectionName { get; set; }
    /// <summary>
    /// Gets or sets the connection string to the MongoDB instance.
    /// </summary>
    string ConnectionString { get; set; }
    /// <summary>
    /// Gets or sets the database name used for the employee store.
    /// </summary>
    string DatabaseName { get; set; }
}


