using EMPLOYEE.MANAGEMENT.CORE.models;
using EMPLOYEE.MANAGEMENT.REPOSITORY.Repository;
using EMPLOYEE.MANAGEMENT.SERVICES.Service;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Net;

ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

var builder = WebApplication.CreateBuilder(args);

// Bind EmployeeStoreDB settings from configuration
builder.Services.Configure<EmployeeStoreDB>(
    builder.Configuration.GetSection(nameof(EmployeeStoreDB)));

// Register EmployeeStoreDB as a singleton using the correct interface
builder.Services.AddSingleton<IEmployeeStoreDB>(sp =>
    sp.GetRequiredService<IOptions<EmployeeStoreDB>>().Value);

// Register MongoDB client using the correct configuration section
builder.Services.AddSingleton<IMongoClient>(s =>
    new MongoClient(builder.Configuration.GetValue<string>("EmployeeStoreDB:ConnectionString")));

// Register your employee service and repository with logging
builder.Services.AddScoped<IEmployeeService, EmployeeService>();
builder.Services.AddScoped<IEmployeeRepository>(sp =>
{
    var client = sp.GetRequiredService<IMongoClient>();
    var settings = sp.GetRequiredService<IEmployeeStoreDB>();
    var logger = sp.GetRequiredService<ILogger<EmployeeRepository>>();
    return new EmployeeRepository(client, settings, logger);
});

// Add controllers and Swagger for API testing
builder.Services.AddControllers();
builder.Services.AddLogging();
builder.Logging.AddProvider(new SimpleFileLoggerProvider("Logs/log.txt"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Enable Swagger in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();

