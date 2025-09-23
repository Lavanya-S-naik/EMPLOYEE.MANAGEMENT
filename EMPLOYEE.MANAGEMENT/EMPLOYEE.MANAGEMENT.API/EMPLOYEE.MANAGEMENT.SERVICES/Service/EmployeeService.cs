using EMPLOYEE.MANAGEMENT.CORE.models;
using EMPLOYEE.MANAGEMENT.REPOSITORY.Repository;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EMPLOYEE.MANAGEMENT.SERVICES.Service
{
    /// <summary>
    /// Service layer that encapsulates business logic for employee operations.
    /// </summary>
    public class EmployeeService : IEmployeeService
    {
        private readonly IEmployeeRepository _repository;
        private readonly ILogger<EmployeeService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeeService"/> class.
        /// </summary>
        /// <param name="repository">The employee repository.</param>
        /// <param name="logger">Logger for service operations.</param>
        public EmployeeService(IEmployeeRepository repository, ILogger<EmployeeService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <summary>
        /// Validates and creates a new employee. Prefer <see cref="Create(Employee)"/>.
        /// </summary>
        /// <param name="employee">The employee to create.</param>
        /// <returns>The created employee.</returns>
        public async Task<Employee> CreateAsync(Employee employee)
        {
            _logger.LogDebug("Validating employee before creation {@Employee}", new { employee?.Name, employee?.Email });
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));
            if (employee.Salary < 0)
                throw new ArgumentException("Salary cannot be negative.");
            await _repository.AddAsync(employee);
            _logger.LogInformation("Employee created with id {EmployeeId}", employee.Id);
            return employee;
        }


        /// <summary>
        /// Creates a new employee after validating input.
        /// </summary>
        /// <param name="employee">The employee to create.</param>
        /// <returns>The created employee.</returns>
        public async Task<Employee> Create(Employee employee)
        {
            _logger.LogDebug("Validating employee before creation {@Employee}", new { employee?.Name, employee?.Email });
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));
            if (string.IsNullOrWhiteSpace(employee.Name))
                throw new ArgumentException("Employee name cannot be null or empty.");
            if (employee.Salary < 0)
                throw new ArgumentException("Salary cannot be negative.");
            await _repository.AddAsync(employee);
            _logger.LogInformation("Employee created with id {EmployeeId}", employee.Id);
            return employee;
        }

        /// <summary>
        /// Retrieves all employees.
        /// </summary>
        public async Task<List<Employee>> Get()
        {
            _logger.LogInformation("Retrieving all employees");
            var result = await _repository.GetAllAsync();
            _logger.LogInformation("Retrieved {EmployeeCount} employees", result?.Count ?? 0);
            return result;
        }

        //public async Task<Employee> Get(string id)
        //{
        //    if (id == null)
        //        throw new ArgumentNullException(nameof(id));
        //    if (string.IsNullOrWhiteSpace(id))
        //        throw new ArgumentNullException(nameof(id));
        //    return await _repository.GetByIdAsync(id);
        //}

        /// <summary>
        /// Retrieves an employee by identifier.
        /// </summary>
        /// <param name="id">The employee identifier.</param>
        public async Task<Employee> Get(string id)
        {
            

            if (string.IsNullOrWhiteSpace(id))
                throw new ArgumentNullException(nameof(id));
            _logger.LogInformation("Retrieving employee {EmployeeId}", id);
            var employee = await _repository.GetByIdAsync(id);
            if (employee == null)
            {
                _logger.LogWarning("Employee {EmployeeId} not found", id);
            }
            else
            {
                _logger.LogInformation("Retrieved employee {EmployeeId}", id);
            }
            return employee;
        }





        /// <summary>
        /// Updates the specified employee.
        /// </summary>
        /// <param name="id">The employee identifier.</param>
        /// <param name="employee">The updated employee data.</param>
        public async Task Update(string id, Employee employee)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));
            _logger.LogInformation("Updating employee {EmployeeId}", id);
            await _repository.UpdateAsync(id, employee);
            _logger.LogInformation("Updated employee {EmployeeId}", id);
        }

        /// <summary>
        /// Removes the specified employee.
        /// </summary>
        /// <param name="id">The employee identifier.</param>
        public async Task Remove(string id)
        {
            if (id == null)
                throw new ArgumentNullException(nameof(id));
            _logger.LogInformation("Removing employee {EmployeeId}", id);
            await _repository.DeleteAsync(id);
            _logger.LogInformation("Removed employee {EmployeeId}", id);
        }
    }
}
