using AutoMapper;
using Data.Model;
using Domain.Service.Impl;
using Microsoft.AspNetCore.Mvc;
using UniqueTrip.Request;
using UniqueTrip.Response;

namespace UniqueTrip.Controllers
{
    /// <summary>
    /// MonthlySchedule Controller
    /// </summary>
    [ApiController]
    [Route("api/v1/MonthlySchedule")]
    public class MonthlyScheduleController : Controller
    {
        private readonly IMonthlyScheduleDomain _monthlyScheduleDomain;
        private readonly IMapper _mapper;

        /// <summary>
        ///  Constructor
        /// </summary>
        /// <param name="monthlyScheduleDomain"></param>
        /// <param name="mapper"></param>
        public MonthlyScheduleController(IMonthlyScheduleDomain monthlyScheduleDomain, IMapper mapper)
        {
            _monthlyScheduleDomain = monthlyScheduleDomain;
            _mapper = mapper;

        }

        /// GET: api/v1/MonthlySchedule/GetAll
        /// <summary>
        /// Get all
        /// Method to get all schedules
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<List<MonthlyScheduleResponse>> GetAll()
        {
            var response = await _monthlyScheduleDomain.GetAll();
            var result = _mapper.Map<List<MonthlySchedule>, List<MonthlyScheduleResponse>>(response);
            return result;
        }

        /// GET: api/v1/MonthlySchedule/GetById/{id}
        /// <summary>
        /// Get by id
        /// Method to get a schedule by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetById/{id}")]
        public async Task<MonthlyScheduleResponse> GetById([FromRoute] int id)
        {
            var response = await _monthlyScheduleDomain.GetById(id);
            var result = _mapper.Map<MonthlySchedule, MonthlyScheduleResponse>(response);
            return result;
        }

        /// GET: api/v1/MonthlySchedule/GetByMonthAndYear?month={month}&year={year}
        /// <summary>
        /// Get by Month and Year
        /// Method to get a schedule by month and year
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpGet("GetByMonthAndYear")]
        public async Task<List<MonthlyScheduleResponse>> GetByMonthAndYear([FromQuery] int month, [FromQuery] int year)
        {
            MonthlySchedule initial = new MonthlySchedule()
            {
                Month = month,
                Year = year
            };
            var response = await _monthlyScheduleDomain.GetByMonthAndYear(initial.Month, initial.Year);
            var result = _mapper.Map<List<MonthlySchedule>, List<MonthlyScheduleResponse>>(response);
            return result;
        }
        /// GET: api/v1/MonthlySchedule/GetByMonthYearClientAndSchedule?month={month}&year={year}&clientId={clientId}&scheduleId={scheduleId}
        /// <summary>
        /// Get by Month, Year, Client and Schedule
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <param name="clientId"></param>
        /// <param name="scheduleId"></param>
        /// <returns></returns>
        [HttpGet("GetByMonthYearClientAndSchedule")]
        public async Task<MonthlyScheduleResponse> GetByMonthYearClientAndSchedule([FromQuery] int month, [FromQuery] int year, [FromQuery] int clientId,[FromQuery] int scheduleId)
        {
            MonthlySchedule initial = new MonthlySchedule()
            {
                Month = month,
                Year = year,
                ClientId = clientId,
                ScheduleId = scheduleId
            };
            var response = await _monthlyScheduleDomain.GetByMonthYearClientAndSchedule(initial.Month, initial.Year, initial.ClientId, initial.ScheduleId);
            var result = _mapper.Map<MonthlySchedule, MonthlyScheduleResponse>(response);
            return result;
        }

        /// GET: api/v1/MonthlySchedule/HasPendingScheduleByMonthAndYear?month={month}&year={year}
        /// <summary>
        /// Get a pending schedule by month and year
        /// </summary>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        [HttpGet("HasPendingScheduleByMonthAndYear")]
        public async Task<IActionResult> HasPendingScheduleByMonthAndYear([FromQuery] int month, [FromQuery] int year)
        {
            try
            {
                var hasPendingSchedule = await _monthlyScheduleDomain.HasPendingScheduleInMonthAndYear(month, year);
                return Ok(hasPendingSchedule);
            }
            catch (Exception ex)
            {
                return StatusCode(500,
                    $"Failed to check for pending schedule in month {month} and year {year}: {ex.Message}");
            }
        }
        
        /// GET: api/v1/MonthlySchedule/Post
        /// <summary>
        /// Post
        /// Method to create a MonthlySchedule
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Post")]
        public async Task<IActionResult> Post([FromBody] MonthlyScheduleRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var result = _mapper.Map<MonthlyScheduleRequest, MonthlySchedule>(request);
                return Ok(await _monthlyScheduleDomain.CreateMonthlySchedule(result.Month, result.Year, result.ClientId, result.ScheduleId));
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }

        /// GET: api/v1/MonthlySchedule/GelAllDebt
        /// <summary>
        /// Get all debts
        /// Method to get all debts
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllDebt")]
        public async Task<IActionResult> GetAllDebt()
        {
            try
            {
                var debtViews = await _monthlyScheduleDomain.GetDebtView();
                Console.WriteLine("Current state of _debtViews:");
                foreach (var debt in debtViews)
                {
                    Console.WriteLine($"Debt Id: {debt.Id}, Client: {debt.ClientName}, MonthlyScheduleId: {debt.MonthlyScheduleId}");
                }
                return Ok(debtViews);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to retrieve debt view: {ex.Message}");
            }
        }
        
        /// PUT: api/v1/MonthlySchedule/Put/{id}
        /// <summary>
        /// Method to update a MonthlySchedule and "Delete" the debt
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("UpdateMonthlyScheduleStatus/{id}")]
        public async Task<IActionResult> UpdateMonthlyScheduleStatus([FromRoute] int id)
        {
            try
            {
                await _monthlyScheduleDomain.UpdateMonthlyScheduleStatus(id);
                return Ok(new { Message = "MonthlySchedule status updated to 'Pagado' successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { Message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = $"Failed to update monthly schedule status: {ex.Message}" });
            }
        }
    }
}