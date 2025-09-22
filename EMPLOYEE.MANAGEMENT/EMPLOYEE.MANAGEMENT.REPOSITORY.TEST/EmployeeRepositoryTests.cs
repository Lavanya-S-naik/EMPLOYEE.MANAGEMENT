using EMPLOYEE.MANAGEMENT.CORE.models;
using EMPLOYEE.MANAGEMENT.REPOSITORY.Repository;
using MongoDB.Driver;
using Moq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;
using System.Linq.Expressions;

public class EmployeeRepositoryTests
{
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
                (ReplaceOptions)null,    // Explicit type: ReplaceOptions
                default                  // CancellationToken
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






}

