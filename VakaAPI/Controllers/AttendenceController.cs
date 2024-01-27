using AutoMapper;
using Exceptions;
using Microsoft.AspNetCore.Mvc;
using VakaAPI.Models;
using VakaAPI.Services;

namespace VakaAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AttendenceController : ControllerBase
    {
        private readonly AttendenceService _attendenceService;
        private readonly IMapper _mapper;

        public AttendenceController(AttendenceService attendenceService)
        {
            _attendenceService = attendenceService;

            _mapper = new Mapper(new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<AttendenceToUpdateDto, Attendence>();
            }));
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Attendence>>> GetAllAttendencesAsync()
        {
            try
            {
                IEnumerable<Attendence> attendences = await _attendenceService.GetAllAsync();
                return Ok(attendences);
            }
            catch (Exception)
            {
                return NotFound("Could not retrieve attendences.");
            }
        }

        [HttpGet("{employeeId}")]
        public async Task<ActionResult<IEnumerable<Attendence>>> GetAttendenceByEmployeeIdAsync(int employeeId)
        {
            try
            {
                IEnumerable<Attendence> attendences = await _attendenceService.GetByEmployeeIdAsync(employeeId);
                if (attendences == null)
                    throw new EntityNotFoundException(employeeId);

                return Ok(attendences);
            }
            catch (Exception)
            {
                return NotFound($"Could not retrieve attendence with employee ID {employeeId}.");
            }
        }

        [HttpPost]
        public async Task<IActionResult> AddAttendenceAsync(Attendence attendence)
        {
            try
            {
                if (await _attendenceService.AddAsync(attendence))
                    return Ok();

                return BadRequest("An error occurred while inserting an attendence.");
            }
            catch (Exception)
            {
                return BadRequest("An error occurred while inserting an attendence.");
            }
        }

        [HttpPut("{employeeId}/{year}/{month}")]
        public async Task<IActionResult> UpdateAttendenceAsync(AttendenceToUpdateDto attendenceDto, int employeeId, int year, int month)
        {
            try
            {
                Attendence attendence = _mapper.Map<Attendence>(attendenceDto);
                attendence.EmployeeId = employeeId;
                attendence.Year = year;
                attendence.Month = month;

                if (await _attendenceService.UpdateAsync(attendence))
                    return Ok();

                return BadRequest("An error occurred while updating an attendence.");
            }
            catch (Exception)
            {
                return BadRequest("An error occurred while updating an attendence.");
            }
        }

        [HttpDelete("{employeeId}")]
        public async Task<IActionResult> DeleteAttendenceAsync(int employeeId)
        {
            try
            {
                if (await _attendenceService.DeleteAsync(employeeId))
                    return Ok();

                return BadRequest("An error occurred while deleting an attendence.");
            }
            catch (Exception)
            {
                return BadRequest("An error occurred while deleting an attendence.");
            }
        }
    }
}
