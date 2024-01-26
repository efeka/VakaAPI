using Exceptions;
using Microsoft.AspNetCore.Mvc;
using VakaAPI.Models;
using VakaAPI.Services;

namespace VakaAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeService _employeeService;

        public EmployeeController(EmployeeService employeeService)
        {
            _employeeService = employeeService;
        }

        [HttpGet("TestConnection")]
        public DateTime TestConnection()
        {
            return DateTime.Now;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetAllEmployeesAsync()
        {
            try
            {
                IEnumerable<Employee> employees = await _employeeService.GetAllAsync();
                return Ok(employees);
            }
            catch (Exception)
            {
                return NotFound("Could not retrieve employees.");
            }
        }

        [HttpGet("{employeeId}")]
        public async Task<ActionResult<Employee>> GetEmployeeByIdAsync(int employeeId)
        {
            try
            {
                Employee? employee = await _employeeService.GetByIdAsync(employeeId);
                if (employee == null)
                    throw new EntityNotFoundException(employeeId);

                return Ok(employee);
            }
            catch (Exception)
            {
                return NotFound($"Could not retrieve employee with ID {employeeId}.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployeeAsync(EmployeeToAddDto employeeDto)
        {
            try
            {
                if (await _employeeService.AddAsync(employeeDto))
                    return Ok();

                return BadRequest("An error occurred while inserting an employee.");
            }
            catch (Exception)
            {
                return BadRequest("An error occurred while inserting an employee.");
            }
        }

        [HttpPut("{employeeId}")]
        public async Task<IActionResult> UpdateEmployeeAsync(EmployeeToAddDto employeeDto, int employeeId)
        {
            try
            {
                if (await _employeeService.UpdateAsync(employeeDto, employeeId))
                    return Ok();

                return BadRequest("An error occurred while updating an employee.");
            }
            catch (Exception)
            {
                return BadRequest("An error occurred while updating an employee.");
            }
        }

        [HttpDelete("{employeeId}")]
        public async Task<IActionResult> DeleteEmployeeAsync(int employeeId)
        {
            try
            {
                if (await _employeeService.DeleteAsync(employeeId))
                    return Ok();

                return BadRequest("An error occurred while deleting an employee.");
            }
            catch (Exception)
            {
                return BadRequest("An error occurred while deleting an employee.");
            }
        }
    }
}
