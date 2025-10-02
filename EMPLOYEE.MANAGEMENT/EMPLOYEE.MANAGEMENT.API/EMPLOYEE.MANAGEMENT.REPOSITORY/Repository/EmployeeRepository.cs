using EMPLOYEE.MANAGEMENT.CORE.models;
using EMPLOYEE.MANAGEMENT.REPOSITORY.Repository;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace EMPLOYEE.MANAGEMENT.REPOSITORY.Repository
{


    /// <summary>
    /// MongoDB-based repository for <see cref="Employee"/> entities.
    /// </summary>
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IMongoCollection<Employee> _employees;
        private readonly ILogger<EmployeeRepository>? _logger;

        /// <summary>
        /// Initializes a new instance using a MongoDB client and store settings.
        /// </summary>
        /// <param name="mongoClient">The MongoDB client.</param>
        /// <param name="settings">The employee store settings.</param>
        public EmployeeRepository(IMongoClient mongoClient, IEmployeeStoreDB settings, ILogger<EmployeeRepository> logger)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _employees = database.GetCollection<Employee>(settings.EmployeesCollectionName);
            _logger = logger;
            // Ensure wrapper is available for read operations
            _wrapper = new EmployeeCollectionWrapper(_employees);
        }


        private readonly IEmployeeCollectionWrapper _wrapper;
        /// <summary>
        /// Initializes a new instance using an abstraction over the employee collection.
        /// </summary>
        /// <param name="wrapper">The employee collection wrapper.</param>
        public EmployeeRepository(IEmployeeCollectionWrapper wrapper)
        {
            _wrapper = wrapper;
        }




        /// <inheritdoc />
        public async Task<List<Employee>> GetAllAsync()
        {
            _logger?.LogInformation("Repository fetching all employees");
            var result = await _wrapper.FindAllAsync();
            _logger?.LogInformation("Repository fetched {EmployeeCount} employees", result?.Count ?? 0);
            return result;
        }

        /// <inheritdoc />
        public async Task<Employee> GetByIdAsync(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            _logger?.LogInformation("Repository fetching employee {EmployeeId}", id);
            var employee = await _wrapper.FindByIdAsync(id);
            if (employee == null)
            {
                _logger?.LogWarning("Repository could not find employee {EmployeeId}", id);
            }
            return employee;
        }

        //public async Task<List<Employee>> GetAllAsync()
        //{
        //    return await _employees.Find(emp => true).ToListAsync();
        //}

        //public async Task<Employee> GetByIdAsync(string id)
        //{
        //    if (string.IsNullOrEmpty(id))
        //        return null;
        //    return await _employees.Find(e => e.Id == id).FirstOrDefaultAsync();
        //}

        /// <inheritdoc />
        public async Task AddAsync(Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));
            _logger?.LogInformation("Repository adding employee {@Employee}", new { employee?.Name, employee?.Email });
            await _employees.InsertOneAsync(employee);
            _logger?.LogInformation("Repository added employee {EmployeeId}", employee.Id);
        }



        /// <inheritdoc />
        public async Task UpdateAsync(string id, Employee employee)
        {
            if (employee == null)
                throw new ArgumentNullException(nameof(employee));
            _logger?.LogInformation("Repository updating employee {EmployeeId}", id);
            await _employees.ReplaceOneAsync(e => e.Id == id, employee);
            _logger?.LogInformation("Repository updated employee {EmployeeId}", id);
        }

        /// <inheritdoc />
        public async Task DeleteAsync(string id)
        {
            _logger?.LogInformation("Repository deleting employee {EmployeeId}", id);
            await _employees.DeleteOneAsync(e => e.Id == id);
            _logger?.LogInformation("Repository deleted employee {EmployeeId}", id);
        }

        public async Task<Employee> GetByEmailAsync(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return null;
            return await _employees.Find(e => e.Email == email).FirstOrDefaultAsync();
        }

    }
}











