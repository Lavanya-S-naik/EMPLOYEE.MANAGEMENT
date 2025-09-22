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

    [Fact]
    public async Task GetById_ReturnsEmployee()
    {
        var repoMock = new Mock<IEmployeeRepository>();
        repoMock.Setup(r => r.GetByIdAsync("2")).ReturnsAsync(new Employee { Id = "2", Name = "John" });
        var service = new EmployeeService(repoMock.Object);

        var employee = await service.Get("2");

        Assert.NotNull(employee);
        Assert.Equal("2", employee.Id);
    }

    [Fact]
    public async Task Create_CallsAddAsync()
    {
        var repoMock = new Mock<IEmployeeRepository>();
        var service = new EmployeeService(repoMock.Object);

        var newEmployee = new Employee { Id = "3", Name = "Alice" };
        repoMock.Setup(r => r.AddAsync(newEmployee)).Returns(Task.CompletedTask);

        await service.Create(newEmployee);

        repoMock.Verify(r => r.AddAsync(newEmployee), Times.Once());
    }

    [Fact]
    public async Task Update_CallsUpdateAsync()
    {
        var repoMock = new Mock<IEmployeeRepository>();
        var service = new EmployeeService(repoMock.Object);

        var updatedEmployee = new Employee { Id = "4", Name = "Bob" };
        repoMock.Setup(r => r.UpdateAsync("4", updatedEmployee)).Returns(Task.CompletedTask);

        await service.Update("4", updatedEmployee);

        repoMock.Verify(r => r.UpdateAsync("4", updatedEmployee), Times.Once());
    }

    [Fact]
    public async Task Remove_CallsDeleteAsync()
    {
        var repoMock = new Mock<IEmployeeRepository>();
        var service = new EmployeeService(repoMock.Object);

        repoMock.Setup(r => r.DeleteAsync("5")).Returns(Task.CompletedTask);

        await service.Remove("5");

        repoMock.Verify(r => r.DeleteAsync("5"), Times.Once());
    }
}
