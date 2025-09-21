using EMPLOYEE.MANAGEMENT.CORE.models;
using EMPLOYEE.MANAGEMENT.REPOSITORY.Repository;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPLOYEE.MANAGEMENT.REPOSITORY.Repository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IMongoCollection<Employee> _employees;

        public EmployeeRepository(IMongoClient mongoClient, IEmployeeStoreDB settings)
        {
            var database = mongoClient.GetDatabase(settings.DatabaseName);
            _employees = database.GetCollection<Employee>(settings.EmployeesCollectionName);
        }

        public async Task<List<Employee>> GetAllAsync()
        {
            return await _employees.Find(emp => true).ToListAsync();
        }

        public async Task<Employee> GetByIdAsync(string id)
        {
            return await _employees.Find(e => e.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddAsync(Employee employee)
        {
            await _employees.InsertOneAsync(employee);
        }

        public async Task UpdateAsync(string id, Employee employee)
        {
            await _employees.ReplaceOneAsync(e => e.Id == id, employee);
        }

        public async Task DeleteAsync(string id)
        {
            await _employees.DeleteOneAsync(e => e.Id == id);
        }
    }
}






