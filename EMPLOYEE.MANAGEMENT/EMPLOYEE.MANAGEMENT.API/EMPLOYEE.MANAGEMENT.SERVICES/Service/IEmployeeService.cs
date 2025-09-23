using EMPLOYEE.MANAGEMENT.CORE.models;
using System.Collections.Generic;


namespace EMPLOYEE.MANAGEMENT.SERVICES.Service
{
    /// <summary>
    /// Defines the operations available for managing employees.
    /// </summary>
    public interface IEmployeeService
    {
        //List<Employee> Get();
        //Employee Get(string id);
        //Employee Create(Employee employee);
        //void Update(string id, Employee employee);
        //void Remove(string id);


        /// <summary>
        /// Retrieves all employees.
        /// </summary>
        Task<List<Employee>> Get();
        /// <summary>
        /// Retrieves an employee by unique identifier.
        /// </summary>
        /// <param name="id">The employee identifier.</param>
        Task<Employee> Get(string id);
        /// <summary>
        /// Creates a new employee.
        /// </summary>
        /// <param name="employee">The employee to create.</param>
        Task<Employee> Create(Employee employee);
        /// <summary>
        /// Updates an existing employee.
        /// </summary>
        /// <param name="id">The employee identifier.</param>
        /// <param name="employee">The employee details to update.</param>
        Task Update(string id, Employee employee);
        /// <summary>
        /// Removes an employee by identifier.
        /// </summary>
        /// <param name="id">The employee identifier.</param>
        Task Remove(string id);


    }
}





