using EMPLOYEE.MANAGEMENT.CORE.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPLOYEE.MANAGEMENT.REPOSITORY.Repository
{
    /// <summary>
    /// Abstraction over the persistence operations for <see cref="Employee"/> entities.
    /// </summary>
    public interface IEmployeeRepository
    {
        /// <summary>
        /// Retrieves all employees.
        /// </summary>
        Task<List<Employee>> GetAllAsync();
        /// <summary>
        /// Retrieves an employee by identifier.
        /// </summary>
        /// <param name="id">The employee identifier.</param>
        Task<Employee> GetByIdAsync(string id);
        /// <summary>
        /// Adds a new employee.
        /// </summary>
        /// <param name="employee">The employee to add.</param>
        Task AddAsync(Employee employee);
        /// <summary>
        /// Updates an existing employee.
        /// </summary>
        /// <param name="id">The employee identifier.</param>
        /// <param name="employee">The updated employee data.</param>
        Task UpdateAsync(string id, Employee employee);
        /// <summary>
        /// Deletes the specified employee.
        /// </summary>
        /// <param name="id">The employee identifier.</param>
        Task DeleteAsync(string id);
    }
}


