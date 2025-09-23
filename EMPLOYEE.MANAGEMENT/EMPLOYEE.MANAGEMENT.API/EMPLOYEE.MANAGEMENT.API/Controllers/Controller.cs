using Microsoft.AspNetCore.Mvc;
using EMPLOYEE.MANAGEMENT.CORE.models;
using EMPLOYEE.MANAGEMENT.SERVICES.Service;
using System.Collections.Generic;
using System.Threading.Tasks; // Needed for async/await and Task

namespace Employee_Management.Controllers
{
    /// <summary>
    /// API controller that exposes CRUD endpoints for managing employees.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ILogger<EmployeesController> _logger;
        private readonly IEmployeeService employeeService;
        //public EmployeesController(IEmployeeService employeeService)
        //{
        //    this.employeeService = employeeService;
        //}

        /// <summary>
        /// Initializes a new instance of the <see cref="EmployeesController"/> class.
        /// </summary>
        /// <param name="logger">The logger instance used for application logging.</param>
        /// <param name="employeeService">The employee service providing data operations.</param>
        public EmployeesController(ILogger<EmployeesController> logger, IEmployeeService employeeService)
        {
            _logger = logger;
            this.employeeService = employeeService;


        }


        /// <summary>
        /// Retrieves all employees.
        /// </summary>
        /// <returns>A list of employees wrapped in an <see cref="ActionResult{TValue}"/>.</returns>
        [HttpGet]
        public async Task<ActionResult<List<Employee>>> Get()
        {
            _logger.LogInformation("Fetching all employees");
            var employees = await employeeService.Get();
            _logger.LogInformation("Fetched {EmployeeCount} employees", employees?.Count ?? 0);
            return Ok(employees);
        }

        /// <summary>
        /// Retrieves a specific employee by unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the employee.</param>
        /// <returns>
        /// The employee when found; otherwise, a <see cref="NotFoundObjectResult"/> with a message.
        /// </returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> Get(string id)
        {
            _logger.LogInformation("Fetching employee by id {EmployeeId}", id);
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogWarning("Invalid id supplied for GET");
                return BadRequest("Id is required.");
            }
            var employee = await employeeService.Get(id);
            if (employee == null)
            {
                _logger.LogWarning("Employee with id {EmployeeId} not found", id);
                return NotFound($"Employee with Id = {id} not found");
            }
           
            _logger.LogInformation("Found employee with id {EmployeeId}", id);
            return Ok(employee);
        }

        /// <summary>
        /// Creates a new employee.
        /// </summary>
        /// <param name="employee">The employee entity to create.</param>
        /// <returns>
        /// A <see cref="CreatedAtActionResult"/> containing the created employee and location header.
        /// </returns>
        [HttpPost]
        public async Task<ActionResult<Employee>> Post([FromBody] Employee employee)
        {
            _logger.LogInformation("Creating new employee {@Employee}", new { employee?.Name, employee?.Email, employee?.Department });
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model for POST: {Errors}", ModelState);
                return ValidationProblem(ModelState);
            }
            await employeeService.Create(employee);
            _logger.LogInformation("Created employee with id {EmployeeId}", employee.Id);
            return CreatedAtAction(nameof(Get), new { id = employee.Id }, employee);
        }

        /// <summary>
        /// Updates an existing employee.
        /// </summary>
        /// <param name="id">The unique identifier of the employee to update.</param>
        /// <param name="employee">The updated employee entity.</param>
        /// <returns>
        /// <see cref="NoContentResult"/> when the update is successful; otherwise <see cref="NotFoundObjectResult"/>.
        /// </returns>
        [HttpPut("{id}")]
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
            var existingEmployee = await employeeService.Get(id);
            if (existingEmployee == null)
            {
                _logger.LogWarning("Cannot update. Employee with id {EmployeeId} not found", id);
                return NotFound($"Employee with Id = {id} not found");
            }
            await employeeService.Update(id, employee);
            _logger.LogInformation("Updated employee {EmployeeId}", id);
            return NoContent();
        }

        /// <summary>
        /// Deletes an employee by unique identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the employee to delete.</param>
        /// <returns>
        /// A <see cref="OkObjectResult"/> with a confirmation message; otherwise <see cref="NotFoundObjectResult"/>.
        /// </returns>
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            _logger.LogInformation("Deleting employee {EmployeeId}", id);
            if (string.IsNullOrWhiteSpace(id))
            {
                _logger.LogWarning("Invalid id supplied for DELETE");
                return BadRequest("Id is required.");
            }
            var employee = await employeeService.Get(id);
            if (employee == null)
            {
                _logger.LogWarning("Cannot delete. Employee with id {EmployeeId} not found", id);
                return NotFound($"Employee with Id = {id} not found");
            }
            await employeeService.Remove(employee.Id);
            _logger.LogInformation("Deleted employee {EmployeeId}", id);
            return Ok($"Employee with Id = {id} deleted");
        }
    }
}
