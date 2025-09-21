using EMPLOYEE.MANAGEMENT.CORE.models;
using EMPLOYEE.MANAGEMENT.REPOSITORY.Repository;
using EMPLOYEE.MANAGEMENT.SERVICES.Service;
using MongoDB.Driver;
using System.Collections.Generic;

namespace EMPLOYEE.MANAGEMENT.SERVICES.Service
{
    public class EmployeeService : IEmployeeService
    {

        private readonly IEmployeeRepository _repository;

        //public EmployeeService(IEmployeeRepository repository)
        //{
        //    _repository = repository;
        //}

        //public List<Employee> Get()
        //{
        //    return _repository.GetAll();
        //}

        //public Employee Get(string id)
        //{
        //    return _repository.GetById(id);
        //}

        //public Employee Create(Employee employee)
        //{
        //    _repository.Add(employee);
        //    return employee;
        //}

        //public void Update(string id, Employee employee)
        //{
        //    _repository.Update(id, employee);
        //}

        //public void Remove(string id)
        //{
        //    _repository.Delete(id);


        //}





        public EmployeeService(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        public async Task<List<Employee>> Get()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<Employee> Get(string id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<Employee> Create(Employee employee)
        {
            await _repository.AddAsync(employee);
            return employee;
        }

        public async Task Update(string id, Employee employee)
        {
            await _repository.UpdateAsync(id, employee);
        }

        public async Task Remove(string id)
        {
            await _repository.DeleteAsync(id);
        }

    }
}



//    private readonly IMongoCollection<Employee> _employees;

//    public EmployeeService(IEmployeeStoreDB settings, IMongoClient mongoClient)
//    {
//        var database = mongoClient.GetDatabase(settings.DatabaseName);
//        _employees = database.GetCollection<Employee>(settings.EmployeesCollectionName);
//    }

//    public Employee Create(Employee employee)
//    {
//        _employees.InsertOne(employee);
//        return employee;
//    }

//    public List<Employee> Get()
//    {
//        return _employees.Find(employee => true).ToList();
//    }

//    public Employee Get(string id)
//    {
//        return _employees.Find(employee => employee.Id == id).FirstOrDefault();
//    }

//    public void Remove(string id)
//    {
//        _employees.DeleteOne(employee => employee.Id == id);
//    }

//    public void Update(string id, Employee employee)
//    {
//        _employees.ReplaceOne(employee => employee.Id == id, employee);
//    }
//}




