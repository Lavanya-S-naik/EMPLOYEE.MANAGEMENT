namespace EMPLOYEE.MANAGEMENT.CORE.models;

public interface IEmployeeStoreDB
{
    string EmployeesCollectionName { get; set; }
    string ConnectionString { get; set; }
    string DatabaseName { get; set; }
}


