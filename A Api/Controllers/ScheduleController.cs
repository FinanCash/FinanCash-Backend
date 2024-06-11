using AutoMapper;
using Data.Model;
using Domain.Service.Impl;
using Microsoft.AspNetCore.Mvc;
using UniqueTrip.Request;
using UniqueTrip.Response;

namespace UniqueTrip.Controllers
{
    /// <summary>
    /// Schedule Controller
    /// </summary>
    [ApiController]
    [Route("api/v1/Schedule")]
    public class ScheduleController : Controller
    {
        private readonly IScheduleDomain _scheduleDomain;
        private readonly IMapper _mapper;
        
        /// <summary>
        ///  Constructor
        /// </summary>
        /// <param name="scheduleDomain"></param>
        /// <param name="mapper"></param>
        public ScheduleController(IScheduleDomain scheduleDomain, IMapper mapper)
        {
            _scheduleDomain = scheduleDomain;
            _mapper = mapper;

        }
        /// GET: api/v1/Schedule/GetAll
        /// <summary>
        /// Get all
        /// Method to get all schedules
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<List<ScheduleResponse>> GetAll()
        {
            var response = await _scheduleDomain.GetAll();
            var result = _mapper.Map<List<Schedule>, List<ScheduleResponse>>(response);
            return result;
        }
        
        /// GET: api/v1/Schedule/GetById/{id}
        /// <summary>
        /// Get by ID
        /// Method to get a schedule by ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetById/{id}")]
        public async Task<ScheduleResponse> GetById([FromRoute] int id)
        {
            var response = await _scheduleDomain.GetById(id);
            var result = _mapper.Map<Schedule, ScheduleResponse>(response);
            return result;
        }
        
        /// POST: api/v1/Schedule/CalculateAndCreateFrenchInstallments?contractId={contractId}&loanId={loanId}
        /// <summary>
        /// Calculate and create French installments for a loan
        /// </summary>
        /// <param name="contractId">The ID of the contract</param>
        /// <param name="loanId">The ID of the loan</param>
        /// <returns>List of calculated installments</returns>
        [HttpPost("CalculateAndCreateFrenchInstallments")]
        public async Task<IActionResult> CalculateAndCreateFrenchInstallments([FromQuery] int contractId, [FromQuery] int loanId)
        {
            try
            {
                var installmentList = await _scheduleDomain.CalculateAndCreateFrenchInstallments(contractId, loanId);
                return Ok(installmentList);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        /// PUT: api/v1/Schedule/Put/{id}
        /// <summary>
        /// Put
        /// Method to update a schedule
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("Put/{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] ScheduleRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var result = _mapper.Map<ScheduleRequest, Schedule >(request);
                return Ok(await _scheduleDomain.Update(result, id));
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }
        
        /// DELETE: api/v1/Schedule/Delete/{id}
        /// <summary>
        /// Delete
        /// Method to delete a schedule 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Delete/{id}")]
        public async Task<bool> Delete(int id)
        {
            return await _scheduleDomain.Delete(id);
        }
    }
}