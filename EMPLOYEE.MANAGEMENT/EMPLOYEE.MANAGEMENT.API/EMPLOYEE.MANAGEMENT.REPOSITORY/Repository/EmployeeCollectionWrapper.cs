using EMPLOYEE.MANAGEMENT.CORE.models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPLOYEE.MANAGEMENT.REPOSITORY.Repository
{
    /// <summary>
    /// Thin wrapper around <see cref="IMongoCollection{TDocument}"/> to facilitate testing.
    /// </summary>
    public class EmployeeCollectionWrapper : IEmployeeCollectionWrapper
    {
        private readonly IMongoCollection<Employee> _employees;
        /// <summary>
        /// Initializes a new instance of the wrapper.
        /// </summary>
        /// <param name="employees">The MongoDB employee collection.</param>
        public EmployeeCollectionWrapper(IMongoCollection<Employee> employees)
        {
            _employees = employees;
        }

        /// <inheritdoc />
        public async Task<List<Employee>> FindAllAsync()
        {
            return await _employees.Find(emp => true).ToListAsync();
        }

        /// <inheritdoc />
        public async Task<Employee> FindByIdAsync(string id)
        {
            return await _employees.Find(e => e.Id == id).FirstOrDefaultAsync();
        }
    }

}
