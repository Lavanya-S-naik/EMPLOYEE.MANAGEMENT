
using Xunit;
using Moq;
using Employee_Management.Controllers;
using EMPLOYEE.MANAGEMENT.SERVICES.Service;
using EMPLOYEE.MANAGEMENT.CORE.models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

public class EmployeesControllerTests
{
    [Fact]
    public async Task Get_ReturnsOkWithEmployees()
    {
        var serviceMock = new Mock<IEmployeeService>();
        serviceMock.Setup(s => s.Get()).ReturnsAsync(new List<Employee>());
        var controller = new EmployeesController(serviceMock.Object);

        var result = await controller.Get();

        Assert.IsType<OkObjectResult>(result.Result);
    }
}

