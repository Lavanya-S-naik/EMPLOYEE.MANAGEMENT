using EMPLOYEE.MANAGEMENT.CORE.models;
using EMPLOYEE.MANAGEMENT.REPOSITORY.Repository;
using MongoDB.Driver;
using Moq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using System.Linq.Expressions;


public class EmployeeRepositoryTests
{
    private EmployeeRepository GetTestRepository(IMongoCollection<Employee> collectionObj = null)
    {
        var collectionMock = collectionObj ?? new Mock<IMongoCollection<Employee>>().Object;
        var clientMock = new Mock<IMongoClient>();
        var dbMock = new Mock<IMongoDatabase>();
        var settingsMock = new Mock<IEmployeeStoreDB>();

        dbMock.Setup(db => db.GetCollection<Employee>(It.IsAny<string>(), null))
            .Returns(collectionMock);

        clientMock.Setup(c => c.GetDatabase(It.IsAny<string>(), null))
            .Returns(dbMock.Object);

        settingsMock.Setup(s => s.DatabaseName).Returns("TestDB");
        settingsMock.Setup(s => s.EmployeesCollectionName).Returns("Employees");

        return new EmployeeRepository(clientMock.Object, settingsMock.Object);
    }

    [Fact]
    public async Task AddAsync_CallsInsertOneAsync()
    {
        var collectionMock = new Mock<IMongoCollection<Employee>>();
        var clientMock = new Mock<IMongoClient>();
        var dbMock = new Mock<IMongoDatabase>();
        var settingsMock = new Mock<IEmployeeStoreDB>();

        dbMock.Setup(db => db.GetCollection<Employee>(It.IsAny<string>(), null)).Returns(collectionMock.Object);
        clientMock.Setup(c => c.GetDatabase(It.IsAny<string>(), null)).Returns(dbMock.Object);
        settingsMock.SetupGet(s => s.DatabaseName).Returns("TestDB");
        settingsMock.SetupGet(s => s.EmployeesCollectionName).Returns("Employees");

        var repo = new EmployeeRepository(clientMock.Object, settingsMock.Object);

        await repo.AddAsync(new Employee { Id = "1", Name = "Test" });
        collectionMock.Verify(c => c.InsertOneAsync(It.IsAny<Employee>(), null, default), Times.Once());
    }

    [Fact]
    public async Task UpdateAsync_CallsReplaceOneAsync()
    {
        var collectionMock = new Mock<IMongoCollection<Employee>>();
        var clientMock = new Mock<IMongoClient>();
        var dbMock = new Mock<IMongoDatabase>();
        var settingsMock = new Mock<IEmployeeStoreDB>();

        dbMock.Setup(db => db.GetCollection<Employee>(It.IsAny<string>(), null)).Returns(collectionMock.Object);
        clientMock.Setup(c => c.GetDatabase(It.IsAny<string>(), null)).Returns(dbMock.Object);
        settingsMock.SetupGet(s => s.DatabaseName).Returns("TestDB");
        settingsMock.SetupGet(s => s.EmployeesCollectionName).Returns("Employees");

        var repo = new EmployeeRepository(clientMock.Object, settingsMock.Object);

        await repo.UpdateAsync("2", new Employee { Id = "2", Name = "Updated" });

        collectionMock.Verify(
            c => c.ReplaceOneAsync(
                It.IsAny<FilterDefinition<Employee>>(),
                It.IsAny<Employee>(),
                (ReplaceOptions)null,
                default
            ),
            Times.Once()
        );
    }

    [Fact]
    public async Task DeleteAsync_CallsDeleteOneAsync()
    {
        var collectionMock = new Mock<IMongoCollection<Employee>>();
        var clientMock = new Mock<IMongoClient>();
        var dbMock = new Mock<IMongoDatabase>();
        var settingsMock = new Mock<IEmployeeStoreDB>();

        dbMock.Setup(db => db.GetCollection<Employee>(It.IsAny<string>(), null)).Returns(collectionMock.Object);
        clientMock.Setup(c => c.GetDatabase(It.IsAny<string>(), null)).Returns(dbMock.Object);
        settingsMock.SetupGet(s => s.DatabaseName).Returns("TestDB");
        settingsMock.SetupGet(s => s.EmployeesCollectionName).Returns("Employees");

        var repo = new EmployeeRepository(clientMock.Object, settingsMock.Object);

        await repo.DeleteAsync("3");

        collectionMock.Verify(c => c.DeleteOneAsync(It.IsAny<FilterDefinition<Employee>>(), default), Times.Once());
    }

    [Fact]
    public async Task AddAsync_WithNullEmployee_ThrowsArgumentNullException()
    {
        var repo = GetTestRepository();
        await Assert.ThrowsAsync<ArgumentNullException>(() => repo.AddAsync(null));
    }

    [Fact]
    public async Task GetByIdAsync_WithEmptyId_ReturnsNull()
    {
        var repo = GetTestRepository();
        var result = await repo.GetByIdAsync(string.Empty);
        Assert.Null(result);
    }

    [Fact]
    public async Task DeleteAsync_WithNonExistentId_DoesNotThrow()
    {
        var repo = GetTestRepository();
        var exception = await Record.ExceptionAsync(() => repo.DeleteAsync("does-not-exist"));
        Assert.Null(exception);
    }

    [Fact]
    public async Task AddAsync_WithBoundaryEmployee_Works()
    {
        var repo = GetTestRepository();
        var employee = new Employee
        {
            Id = "1",
            Name = new string('A', 5000),
            Department = new string('B', 5000),
            Email = "test@example.com",
            Salary = decimal.MaxValue,
            DateOfJoining = DateTime.MaxValue,
            IsActive = false
        };
        await repo.AddAsync(employee);
    }

    [Fact]
    public async Task UpdateAsync_WithNullEmployee_ThrowsArgumentNullException()
    {
        var repo = GetTestRepository();
        await Assert.ThrowsAsync<ArgumentNullException>(() => repo.UpdateAsync("123", null));
    }


    [Fact]
    public async Task GetAllAsync_EmptyCollection_ReturnsEmptyList()
    {
        var wrapperMock = new Mock<IEmployeeCollectionWrapper>();
        wrapperMock.Setup(w => w.FindAllAsync()).ReturnsAsync(new List<Employee>());

        var repo = new EmployeeRepository(wrapperMock.Object);

        var result = await repo.GetAllAsync();
        Assert.Empty(result);
    }

    [Fact]
    public async Task GetByIdAsync_WithValidId_ReturnsEmployee()
    {
        var employee = new Employee { Id = "valid-id", Name = "Valid User" };
        var wrapperMock = new Mock<IEmployeeCollectionWrapper>();
        wrapperMock.Setup(w => w.FindByIdAsync("valid-id")).ReturnsAsync(employee);

        var repo = new EmployeeRepository(wrapperMock.Object);

        var result = await repo.GetByIdAsync("valid-id");
        Assert.NotNull(result);
        Assert.Equal("valid-id", result.Id);
    }





}
