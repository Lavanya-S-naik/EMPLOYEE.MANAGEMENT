using Xunit;
using Moq;
using EMPLOYEE.MANAGEMENT.SERVICES.Service;
using EMPLOYEE.MANAGEMENT.REPOSITORY.Repository;
using EMPLOYEE.MANAGEMENT.CORE.models;
using System.Collections.Generic;
using System.Threading.Tasks;

public class EmployeeServiceTests
{
    [Fact]
    public async Task Get_ReturnsAllEmployees()
    {
        var repoMock = new Mock<IEmployeeRepository>();
        repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Employee> { new Employee { Id = "1", Name = "Test" } });
        var service = new EmployeeService(repoMock.Object);

        var employees = await service.Get();

        Assert.Single(employees);
        Assert.Equal("1", employees[0].Id);
    }
}
