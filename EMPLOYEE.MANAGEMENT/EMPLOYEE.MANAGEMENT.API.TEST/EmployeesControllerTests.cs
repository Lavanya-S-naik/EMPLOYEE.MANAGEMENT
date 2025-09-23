
using EMPLOYEE.MANAGEMENT.CORE.models;
using EMPLOYEE.MANAGEMENT.SERVICES.Service;
using Employee_Management.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class EmployeesControllerTests
{
    [Fact]
    public async Task Get_ReturnsOkWithEmployees()
    {
        var serviceMock = new Mock<IEmployeeService>();
        serviceMock.Setup(s => s.Get()).ReturnsAsync(new List<Employee>());
        //var controller = new EmployeesController(serviceMock.Object);

        var loggerMock = new Mock<ILogger<EmployeesController>>();
        var controller = new EmployeesController(loggerMock.Object, serviceMock.Object);





        var result = await controller.Get();

        Assert.IsType<OkObjectResult>(result.Result);
    }

    [Fact]
    public async Task Get_ById_ReturnsOk_WhenEmployeeExists()
    {
        var serviceMock = new Mock<IEmployeeService>();
        var testEmp = new Employee { Id = "1", Name = "Alice" };
        serviceMock.Setup(s => s.Get("1")).ReturnsAsync(testEmp);

        var loggerMock = new Mock<ILogger<EmployeesController>>();
        var controller = new EmployeesController(loggerMock.Object, serviceMock.Object);

 

        var result = await controller.Get("1");
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var employee = Assert.IsType<Employee>(okResult.Value);
        Assert.Equal("1", employee.Id);

    }

    [Fact]
    public async Task Get_ById_ReturnsNotFound_WhenEmployeeDoesNotExist()
    {
        var serviceMock = new Mock<IEmployeeService>();
        serviceMock.Setup(s => s.Get(It.IsAny<string>()))
            .ReturnsAsync((Employee)null);

        var loggerMock = new Mock<ILogger<EmployeesController>>();
        var controller = new EmployeesController(loggerMock.Object, serviceMock.Object);



        var result = await controller.Get("unknown-id");
        Assert.IsType<NotFoundObjectResult>(result.Result);

    }

    [Fact]
    public async Task Post_CreatesEmployee_ReturnsCreatedAtAction()
    {
        var serviceMock = new Mock<IEmployeeService>();
     

        serviceMock.Setup(s => s.Create(It.IsAny<Employee>())).ReturnsAsync(new Employee { Id = "2", Name = "Bob" });



        var loggerMock = new Mock<ILogger<EmployeesController>>();
        var controller = new EmployeesController(loggerMock.Object, serviceMock.Object);

        var newEmp = new Employee { Id = "2", Name = "Bob" };


        var result = await controller.Post(newEmp);
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        Assert.Equal(newEmp, createdResult.Value);

    }

    [Fact]
    public async Task Put_UpdatesEmployee_ReturnsNoContent_WhenFound()
    {
        var serviceMock = new Mock<IEmployeeService>();
        var existingEmp = new Employee { Id = "1", Name = "Alice" };
        serviceMock.Setup(s => s.Get("1")).ReturnsAsync(existingEmp);
        serviceMock.Setup(s => s.Update("1", It.IsAny<Employee>())).Returns(Task.CompletedTask);

        var loggerMock = new Mock<ILogger<EmployeesController>>();
        var controller = new EmployeesController(loggerMock.Object, serviceMock.Object);

        var result = await controller.Put("1", new Employee { Id = "1", Name = "Updated" });

        Assert.IsType<NoContentResult>(result);
    }

    [Fact]
    public async Task Put_EmployeeNotFound_ReturnsNotFound()
    {
        var serviceMock = new Mock<IEmployeeService>();
        serviceMock.Setup(s => s.Get(It.IsAny<string>())).ReturnsAsync((Employee)null);

        var loggerMock = new Mock<ILogger<EmployeesController>>();
        var controller = new EmployeesController(loggerMock.Object, serviceMock.Object);

        var result = await controller.Put("1", new Employee { Id = "1", Name = "Updated" });

        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task Delete_RemovesEmployee_ReturnsOk_WhenFound()
    {
        var serviceMock = new Mock<IEmployeeService>();
        var emp = new Employee { Id = "1", Name = "Alice" };
        serviceMock.Setup(s => s.Get("1")).ReturnsAsync(emp);
        serviceMock.Setup(s => s.Remove("1")).Returns(Task.CompletedTask);



        var loggerMock = new Mock<ILogger<EmployeesController>>();
        var controller = new EmployeesController(loggerMock.Object, serviceMock.Object);


        var result = await controller.Delete("1");

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Contains("deleted", okResult.Value.ToString());
    }

    [Fact]
    public async Task Delete_EmployeeNotFound_ReturnsNotFound()
    {
        var serviceMock = new Mock<IEmployeeService>();
        serviceMock.Setup(s => s.Get(It.IsAny<string>())).ReturnsAsync((Employee)null);

        var loggerMock = new Mock<ILogger<EmployeesController>>();
        var controller = new EmployeesController(loggerMock.Object, serviceMock.Object);

        var result = await controller.Delete("1");

        Assert.IsType<NotFoundObjectResult>(result);
    }


}

