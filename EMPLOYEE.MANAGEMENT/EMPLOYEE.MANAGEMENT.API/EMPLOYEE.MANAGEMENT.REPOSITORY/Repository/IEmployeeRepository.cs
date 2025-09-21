using EMPLOYEE.MANAGEMENT.CORE.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMPLOYEE.MANAGEMENT.REPOSITORY.Repository
{
    public interface IEmployeeRepository
    {
        Task<List<Employee>> GetAllAsync();
        Task<Employee> GetByIdAsync(string id);
        Task AddAsync(Employee employee);
        Task UpdateAsync(string id, Employee employee);
        Task DeleteAsync(string id);
    }
}


