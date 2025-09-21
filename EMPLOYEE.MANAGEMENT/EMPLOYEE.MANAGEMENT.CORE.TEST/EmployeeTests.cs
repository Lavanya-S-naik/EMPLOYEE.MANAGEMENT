using Xunit;
using EMPLOYEE.MANAGEMENT.CORE.models;

public class EmployeeTests
{
    [Fact]
    public void Employee_CreatedWithDefaultValues_IsActiveTrue()
    {
        var employee = new Employee();
        Assert.True(employee.IsActive);
    }
}
