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


    [Fact]
    public async Task CreateAsync_WithNullEmployee_ThrowsArgumentNullException()
    {
        var repoMock = new Mock<IEmployeeRepository>();
        var service = new EmployeeService(repoMock.Object);

        await Assert.ThrowsAsync<ArgumentNullException>(() => service.CreateAsync(null));
    }




    [Fact]
    public async Task RemoveAsync_WithNonExistentId_DoesNotThrow()
    {
        var repoMock = new Mock<IEmployeeRepository>();
        repoMock.Setup(r => r.DeleteAsync(It.IsAny<string>())).Returns(Task.CompletedTask);
        var service = new EmployeeService(repoMock.Object);
        var exception = await Record.ExceptionAsync(() => service.Remove("does-not-exist"));
        Assert.Null(exception);
    }

    [Fact]
    public async Task CreateAsync_WithBoundaryEmployee_Works()
    {
        var repoMock = new Mock<IEmployeeRepository>();
        repoMock.Setup(r => r.AddAsync(It.IsAny<Employee>())).Returns(Task.CompletedTask);
        var service = new EmployeeService(repoMock.Object);

        var employee = new Employee
        {
            Id = "id",
            Name = new string('A', 5000),
            Department = "Z",
            Salary = decimal.MaxValue,
            DateOfJoining = DateTime.MaxValue,
            IsActive = true
        };
        var result = await service.Create(employee);
        Assert.Equal(employee, result);
    }

    [Fact]
    public async Task Get_ReturnsEmptyList_WhenNoEmployees()
    {
        var repoMock = new Mock<IEmployeeRepository>();
        repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Employee>());
        var service = new EmployeeService(repoMock.Object);

        var employees = await service.Get();

        Assert.Empty(employees);
    }

    [Fact]
    public async Task GetById_ReturnsNull_WhenIdNotFound()
    {
        var repoMock = new Mock<IEmployeeRepository>();
        repoMock.Setup(r => r.GetByIdAsync("unknown")).ReturnsAsync((Employee)null);
        var service = new EmployeeService(repoMock.Object);

        var employee = await service.Get("unknown");

        Assert.Null(employee);
    }
    [Fact]
    public async Task UpdateAsync_WithNonExistentId_DoesNotThrow()
    {
        var repoMock = new Mock<IEmployeeRepository>();
        repoMock.Setup(r => r.UpdateAsync("doesnotexist", It.IsAny<Employee>())).Returns(Task.CompletedTask);
        var service = new EmployeeService(repoMock.Object);
        var emp = new Employee { Id = "doesnotexist", Name = "Person" };

        var ex = await Record.ExceptionAsync(() => service.Update("doesnotexist", emp));
        Assert.Null(ex);
    }
    [Fact]
    public async Task RemoveAsync_WithNullId_ThrowsArgumentNullException()
    {
        var repoMock = new Mock<IEmployeeRepository>();
        var service = new EmployeeService(repoMock.Object);

        await Assert.ThrowsAsync<ArgumentNullException>(() => service.Remove(null));
    }
    [Fact]
    public async Task UpdateAsync_WithNullEmployee_ThrowsArgumentNullException()
    {
        var repoMock = new Mock<IEmployeeRepository>();
        var service = new EmployeeService(repoMock.Object);

        await Assert.ThrowsAsync<ArgumentNullException>(() => service.Update("id", null));
    }
    [Fact]
    public async Task Create_WithNegativeSalary_ThrowsArgumentException()
    {
        var repoMock = new Mock<IEmployeeRepository>();
        var service = new EmployeeService(repoMock.Object);
        var emp = new Employee { Id = "bad", Name = "Low", Salary = -1000m };

        // only if you add this logic to EmployeeService.Create
        await Assert.ThrowsAsync<ArgumentException>(() => service.Create(emp));
    }
    [Fact]
    public async Task GetAsync_WithNullId_ThrowsArgumentNullException()
    {
        var repoMock = new Mock<IEmployeeRepository>();
        var service = new EmployeeService(repoMock.Object);

        await Assert.ThrowsAsync<ArgumentNullException>(() => service.Get(null));
    }
    [Fact]
    public async Task Remove_CallsRepoDeleteAsyncOnce()
    {
        var repoMock = new Mock<IEmployeeRepository>();
        repoMock.Setup(r => r.DeleteAsync("id")).Returns(Task.CompletedTask);

        var service = new EmployeeService(repoMock.Object);
        await service.Remove("id");
        repoMock.Verify(r => r.DeleteAsync("id"), Times.Once());
    }
    [Fact]
    public async Task UpdateAsync_ValidEmployee_UpdatesSuccessfully()
    {
        var repoMock = new Mock<IEmployeeRepository>();
        repoMock.Setup(r => r.UpdateAsync("id", It.IsAny<Employee>())).Returns(Task.CompletedTask);

        var service = new EmployeeService(repoMock.Object);
        var emp = new Employee { Id = "id", Name = "Updated" };

        await service.Update("id", emp);
        repoMock.Verify(r => r.UpdateAsync("id", emp), Times.Once());
    }
    [Fact]
    public async Task Remove_NonexistentEmployee_DoesNotThrow()
    {
        var repoMock = new Mock<IEmployeeRepository>();
        repoMock.Setup(r => r.DeleteAsync(It.IsAny<string>())).Returns(Task.CompletedTask);

        var service = new EmployeeService(repoMock.Object);
        var ex = await Record.ExceptionAsync(() => service.Remove("xyz"));
        Assert.Null(ex);
    }
    [Fact]
    public async Task Create_WithNullName_ThrowsArgumentException()
    {
        var repoMock = new Mock<IEmployeeRepository>();
        var service = new EmployeeService(repoMock.Object);
        var emp = new Employee { Id = "new", Name = null };

        // If the logic is present in service
        await Assert.ThrowsAsync<ArgumentException>(() => service.Create(emp));
    }
    //[Fact]
    //public async Task GetAsync_WhitespaceId_ThrowsArgumentNullException()
    //{
    //    var repoMock = new Mock<IEmployeeRepository>();
    //    var service = new EmployeeService(repoMock.Object);

    //    await Assert.ThrowsAsync<ArgumentNullException>(() => service.Get("   "));
    //}

    [Fact]
    public async Task UpdateAsync_NegativeSalary_ThrowsArgumentException()
    {
        var repoMock = new Mock<IEmployeeRepository>();
        var service = new EmployeeService(repoMock.Object);
        var emp = new Employee { Id = "bad", Name = "Person", Salary = -1 };

    }








}
