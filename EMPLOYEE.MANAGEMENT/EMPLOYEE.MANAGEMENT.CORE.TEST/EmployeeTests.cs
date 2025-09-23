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

    [Fact]
    public void Employee_DefaultValues_Correct()
    {
        var emp = new Employee();
        Assert.Equal(string.Empty, emp.Id);
        Assert.Equal(string.Empty, emp.Name);
        Assert.Equal(string.Empty, emp.Department);
        Assert.Equal(string.Empty, emp.Position);
        Assert.Equal(string.Empty, emp.Email);
        Assert.Equal(string.Empty, emp.Phone);
        Assert.Equal(0m, emp.Salary);
        Assert.True(emp.IsActive);
        Assert.Equal(default(DateTime), emp.DateOfJoining);
    }

    [Fact]
    public void Employee_SetProperties_StoresValuesCorrectly()
    {
        var date = new DateTime(2022, 5, 1);
        var emp = new Employee
        {
            Id = "123abc",
            Name = "John Doe",
            Department = "Finance",
            Position = "Analyst",
            Email = "john@company.com",
            Phone = "123456789",
            Salary = 50000m,
            DateOfJoining = date,
            IsActive = false
        };

        Assert.Equal("123abc", emp.Id);
        Assert.Equal("John Doe", emp.Name);
        Assert.Equal("Finance", emp.Department);
        Assert.Equal("Analyst", emp.Position);
        Assert.Equal("john@company.com", emp.Email);
        Assert.Equal("123456789", emp.Phone);
        Assert.Equal(50000m, emp.Salary);
        Assert.Equal(date, emp.DateOfJoining);
        Assert.False(emp.IsActive);
    }
    [Fact]
    public void Employee_MissingFields_UsesDefaults()
    {
        var emp = new Employee();
        Assert.True(string.IsNullOrEmpty(emp.Name));
        Assert.True(emp.IsActive);
    }
    [Fact]
    public void Employee_HighSalaryAllowed()
    {
        var emp = new Employee { Salary = decimal.MaxValue };
        Assert.Equal(decimal.MaxValue, emp.Salary);
    }

    [Fact]
    public void Employee_EmailFormat_StoresString()
    {
        var emp = new Employee { Email = "test@company.com" };
        Assert.Contains("@", emp.Email);
        // Add more checks if you use a regex validator in your logic
    }
    [Fact]
    public void Employee_ActiveInactiveToggle_Works()
    {
        var emp = new Employee();
        Assert.True(emp.IsActive);
        emp.IsActive = false;
        Assert.False(emp.IsActive);
    }
    [Fact]
    public void Employee_NullStringAssignment_Works()
    {
        var emp = new Employee { Name = null, Email = null };
        Assert.Null(emp.Name);
        Assert.Null(emp.Email);
    }
    [Fact]
    public void Employee_NegativeSalaryAllowed()
    {
        var emp = new Employee { Salary = -10000m };
        Assert.Equal(-10000m, emp.Salary);
        // If you want to prohibit, add validation logic in your class and test for exception accordingly
    }
    [Fact]
    public void Employee_ToString_ContainsNameAndId()
    {
        var emp = new Employee { Id = "test", Name = "Bob" };
        Assert.Contains("Bob", emp.ToString());
        Assert.Contains("test", emp.ToString());
    }
    [Fact]
    public void Employee_LongDepartmentName_Allowed()
    {
        var longName = new string('A', 1000);
        var emp = new Employee { Department = longName };
        Assert.Equal(longName, emp.Department);
    }
    [Fact]
    public void Employee_DateOfJoining_Assignment_Works()
    {
        var joinDate = new DateTime(2020, 10, 29);
        var emp = new Employee { DateOfJoining = joinDate };
        Assert.Equal(joinDate, emp.DateOfJoining);
    }

    [Fact]
    public void Employee_DefaultIsActiveIsTrue()
    {
        var emp = new Employee();
        Assert.True(emp.IsActive);
    }
    [Fact]
    public void Employee_DataConsistency()
    {
        var emp = new Employee
        {
            Name = "Sam",
            Position = "Manager",
            Department = "Operations",
            Email = "sam@company.com",
            Phone = "9876543210",
            IsActive = true
        };
        Assert.True(emp.IsActive);
        Assert.Contains("sam", emp.Email);
        Assert.Equal("Manager", emp.Position);
    }
    [Fact]
    public void Employee_MaxFieldLengths_Accepted()
    {
        var longString = new string('X', 4000);
        var emp = new Employee { Name = longString, Department = longString, Position = longString, Email = longString, Phone = longString };
        Assert.Equal(longString, emp.Name);
        Assert.Equal(longString, emp.Department);
        Assert.Equal(longString, emp.Position);
        Assert.Equal(longString, emp.Email);
        Assert.Equal(longString, emp.Phone);
    }
    [Fact]
    public void Employee_ChangeDepartment_Works()
    {
        var emp = new Employee { Department = "Finance" };
        emp.Department = "IT";
        Assert.Equal("IT", emp.Department);
    }
    [Fact]
    public void Employee_Id_CanBeMongoObjectIdString()
    {
        // Simulate a Mongo ObjectId string
        var emp = new Employee { Id = "507f1f77bcf86cd799439011" };
        Assert.Equal("507f1f77bcf86cd799439011", emp.Id);
    }
    [Fact]
    public void Employee_Salary_CanHaveDecimalPrecision()
    {
        var emp = new Employee { Salary = 12345.67m };
        Assert.Equal(12345.67m, emp.Salary);
    }
    [Fact]
    public void Employee_MultiplePropertyUpdate()
    {
        var emp = new Employee();
        emp.Name = "Jane";
        emp.Department = "HR";
        emp.Position = "Lead";
        emp.Email = "jane@company.com";
        emp.Phone = "1122334455";
        emp.Salary = 89000m;
        emp.DateOfJoining = new DateTime(2021, 2, 14);
        emp.IsActive = false;

        Assert.Equal("Jane", emp.Name);
        Assert.Equal("HR", emp.Department);
        Assert.Equal("Lead", emp.Position);
        Assert.Equal("jane@company.com", emp.Email);
        Assert.Equal("1122334455", emp.Phone);
        Assert.Equal(89000m, emp.Salary);
        Assert.Equal(new DateTime(2021, 2, 14), emp.DateOfJoining);
        Assert.False(emp.IsActive);
    }
    [Fact]
    public void Employee_DefaultConstructor_AllDefaults()
    {
        var emp = new Employee();
        Assert.Equal(string.Empty, emp.Name);
        Assert.Equal(string.Empty, emp.Department);
        Assert.True(emp.IsActive);
    }
    [Fact]
    public void Employee_EmailCaseSensitive()
    {
        var emp = new Employee { Email = "TEST@COMPANY.COM" };
        Assert.Equal("TEST@COMPANY.COM", emp.Email);
        emp.Email = "test@company.com";
        Assert.Equal("test@company.com", emp.Email);
    }
    [Fact]
    public void Employee_LeapYearDateOfJoining()
    {
        var leap = new DateTime(2024, 2, 29);
        var emp = new Employee { DateOfJoining = leap };
        Assert.Equal(leap, emp.DateOfJoining);
    }
    [Fact]
    public void Employee_MinSalary()
    {
        var emp = new Employee { Salary = 0m };
        Assert.Equal(0m, emp.Salary);
    }






}

