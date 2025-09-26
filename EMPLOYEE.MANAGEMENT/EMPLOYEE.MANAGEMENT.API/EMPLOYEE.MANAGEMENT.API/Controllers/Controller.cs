using EMPLOYEE.MANAGEMENT.CORE.models;
using EMPLOYEE.MANAGEMENT.REPOSITORY.Repository;
using EMPLOYEE.MANAGEMENT.SERVICES.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Employee_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize] // Require JWT authentication for all endpoints
    public class EmployeesController : ControllerBase
    {
        private readonly ILogger<EmployeesController> _logger;
        private readonly IEmployeeService _employeeService;
        private readonly UserRepository _userRepository;

        // Inject all dependencies in one constructor
        public EmployeesController(
            ILogger<EmployeesController> logger,
            IEmployeeService employeeService,
            UserRepository userRepository)
        {
            _logger = logger;
            _employeeService = employeeService;
            _userRepository = userRepository;
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Moderator,ReadOnly")]
        public async Task<ActionResult<List<Employee>>> Get()
        {
            _logger.LogInformation("Fetching all employees");
            var employees = await _employeeService.Get();
            _logger.LogInformation("Fetched {EmployeeCount} employees", employees?.Count ?? 0);
            return Ok(employees);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,Moderator,ReadOnly")]
        public async Task<ActionResult<Employee>> Get(string id)
        {
            _logger.LogInformation("Fetching employee by id {EmployeeId}", id);
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogWarning("Invalid id supplied for GET");
                return BadRequest("Id is required.");
            }
            var employee = await _employeeService.Get(id);
            if (employee == null)
            {
                _logger.LogWarning("Employee with id {EmployeeId} not found", id);
                return NotFound($"Employee with Id = {id} not found");
            }
            _logger.LogInformation("Found employee with id {EmployeeId}", id);
            return Ok(employee);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult<Employee>> Post([FromBody] Employee employee)
        {
            _logger.LogInformation("Creating new employee {@Employee}", new { employee?.Name, employee?.Email, employee?.Department });
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model for POST: {Errors}", ModelState);
                return ValidationProblem(ModelState);
            }
            await _employeeService.Create(employee);
            _logger.LogInformation("Created employee with id {EmployeeId}", employee.Id);
            return CreatedAtAction(nameof(Get), new { id = employee.Id }, employee);
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin,Moderator")]
        public async Task<ActionResult> Put(string id, [FromBody] Employee employee)
        {
            _logger.LogInformation("Updating employee {EmployeeId}", id);
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogWarning("Invalid id supplied for PUT");
                return BadRequest("Id is required.");
            }
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model for PUT: {Errors}", ModelState);
                return ValidationProblem(ModelState);
            }
            var existingEmployee = await _employeeService.Get(id);
            if (existingEmployee == null)
            {
                _logger.LogWarning("Cannot update. Employee with id {EmployeeId} not found", id);
                return NotFound($"Employee with Id = {id} not found");
            }
            await _employeeService.Update(id, employee);
            _logger.LogInformation("Updated employee {EmployeeId}", id);
            return NoContent();
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Delete(string id)
        {
            _logger.LogInformation("Deleting employee {EmployeeId}", id);
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogWarning("Invalid id supplied for DELETE");
                return BadRequest("Id is required.");
            }
            var employee = await _employeeService.Get(id);
            if (employee == null)
            {
                _logger.LogWarning("Cannot delete. Employee with id {EmployeeId} not found", id);
                return NotFound($"Employee with Id = {id} not found");
            }
            await _employeeService.Remove(employee.Id);
            _logger.LogInformation("Deleted employee {EmployeeId}", id);
            return Ok($"Employee with Id = {id} deleted");
        }
    }
}
