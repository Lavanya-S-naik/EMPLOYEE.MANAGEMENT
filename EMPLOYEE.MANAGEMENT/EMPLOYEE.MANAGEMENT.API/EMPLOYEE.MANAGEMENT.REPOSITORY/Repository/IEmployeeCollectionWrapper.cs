using EMPLOYEE.MANAGEMENT.CORE.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPLOYEE.MANAGEMENT.REPOSITORY.Repository
{



    /// <summary>
    /// Abstraction over MongoDB collection used by the repository for testability.
    /// </summary>
    public interface IEmployeeCollectionWrapper
    {
        /// <summary>
        /// Finds and returns all employees.
        /// </summary>
        Task<List<Employee>> FindAllAsync();
        /// <summary>
        /// Finds and returns an employee by identifier.
        /// </summary>
        /// <param name="id">The employee identifier.</param>
        Task<Employee> FindByIdAsync(string id);
    }
}

