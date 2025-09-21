
namespace EMPLOYEE.MANAGEMENT.CORE.models;


public class EmployeeStoreDB : IEmployeeStoreDB
{
    public string EmployeesCollectionName { get; set; } = string.Empty;
    public string ConnectionString { get; set; } = string.Empty;
    public string DatabaseName { get; set; } = string.Empty;
}

