using Microsoft.AspNetCore.Mvc;
using EMPLOYEE.MANAGEMENT.CORE.models;
using EMPLOYEE.MANAGEMENT.SERVICES.Service;
using System.Collections.Generic;
using System.Threading.Tasks; // Needed for async/await and Task

namespace Employee_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeService employeeService;
        public EmployeesController(IEmployeeService employeeService)
        {
            this.employeeService = employeeService;
        }

        // GET: api/<EmployeesController>
        [HttpGet]
        public async Task<ActionResult<List<Employee>>> Get()
        {
            var employees = await employeeService.Get();
            return Ok(employees);
        }

        // GET api/<EmployeesController>/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> Get(string id)
        {
            var employee = await employeeService.Get(id);
            if (employee == null)
            {
                return NotFound($"Employee with Id = {id} not found");
            }
            return Ok(employee);
        }

        // POST api/<EmployeesController>
        [HttpPost]
        public async Task<ActionResult<Employee>> Post([FromBody] Employee employee)
        {
            await employeeService.Create(employee);
            return CreatedAtAction(nameof(Get), new { id = employee.Id }, employee);
        }

        // PUT api/<EmployeesController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(string id, [FromBody] Employee employee)
        {
            var existingEmployee = await employeeService.Get(id);
            if (existingEmployee == null)
            {
                return NotFound($"Employee with Id = {id} not found");
            }
            await employeeService.Update(id, employee);
            return NoContent();
        }

        // DELETE api/<EmployeesController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(string id)
        {
            var employee = await employeeService.Get(id);
            if (employee == null)
            {
                return NotFound($"Employee with Id = {id} not found");
            }
            await employeeService.Remove(employee.Id);
            return Ok($"Employee with Id = {id} deleted");
        }
    }
}
