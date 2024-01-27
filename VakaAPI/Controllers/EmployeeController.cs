using AutoMapper;
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
        private readonly IMapper _mapper;

        public EmployeeController(EmployeeService employeeService)
        {
            _employeeService = employeeService;

            _mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<EmployeeToAddDto, Employee>();
            }));
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

        [HttpGet("/IncludeSalaries/{year}/{month}")]
        public async Task<ActionResult<IEnumerable<EmployeeWithSalaryDto>>> GetEmployeesWithSalariesByDate(int year, int month)
        {
            try
            {
                if (year < 1 || year > 9999)
                    return BadRequest("Year must be between 1 and 9999.");
                if (month < 1 || month > 12)
                    return BadRequest("Month must be between 1 and 12.");

                IEnumerable<EmployeeWithSalaryDto> employees = await _employeeService.GetAllWithSalariesByDate(
                    new DateTime(year: year, month: month, day: 1)
                );
                return Ok(employees);
            }
            catch (Exception)
            {
                return NotFound("Could not retrieve employees with salaries.");
            }
        }

        [HttpGet("/IncludeSalaries/{employeeId}")]
        public async Task<ActionResult<IEnumerable<EmployeeWithSalaryDto>>> GetEmployeeSalariesById(int employeeId)
        {
            try
            {
                IEnumerable<EmployeeWithSalaryDto> employeeSalaries = await _employeeService.GetWithSalariesById(employeeId);
                return Ok(employeeSalaries);
            }
            catch (Exception)
            {
                return NotFound($"Could not retrieve salaries for employee with ID {employeeId}.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployeeAsync(EmployeeToAddDto employeeDto)
        {
            try
            {
                Employee employee = _mapper.Map<Employee>(employeeDto);
                if (await _employeeService.AddAsync(employee))
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
                Employee employee = _mapper.Map<Employee>(employeeDto);
                employee.EmployeeId = employeeId;
                if (await _employeeService.UpdateAsync(employee))
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
