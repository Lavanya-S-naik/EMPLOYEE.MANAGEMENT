using EMPLOYEE.MANAGEMENT.CORE.models;
using System.Collections.Generic;


namespace EMPLOYEE.MANAGEMENT.SERVICES.Service
{
    public interface IEmployeeService
    {
        //List<Employee> Get();
        //Employee Get(string id);
        //Employee Create(Employee employee);
        //void Update(string id, Employee employee);
        //void Remove(string id);


        Task<List<Employee>> Get();
        Task<Employee> Get(string id);
        Task<Employee> Create(Employee employee);
        Task Update(string id, Employee employee);
        Task Remove(string id);


    }
}





