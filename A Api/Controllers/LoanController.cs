using AutoMapper;
using Data.Model;
using Domain.Service.Impl;
using Microsoft.AspNetCore.Mvc;
using UniqueTrip.Request;
using UniqueTrip.Response;

namespace UniqueTrip.Controllers
{
    /// <summary>
    /// Loan Controller
    /// </summary>
    [ApiController]
    [Route("api/v1/Loan")]
    public class LoanController : Controller
    {
        private readonly ILoanDomain _loanDomain;
        private readonly IMapper _mapper;
        
        /// <summary>
        ///  Constructor
        /// </summary>
        /// <param name="loanDomain"></param>
        /// <param name="mapper"></param>
        public LoanController(ILoanDomain loanDomain, IMapper mapper)
        { 
            _loanDomain = loanDomain;
            _mapper = mapper;

        }
        /// GET: api/v1/Loan/GetAll
        /// <summary>
        /// Get All
        /// Method to get all loans
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<List<LoanResponse>> GetAll()
        {
            var response = await _loanDomain.GetAll();
            var result = _mapper.Map<List<Loan>, List<LoanResponse>>(response);
            return result;
        }

        /// GET: api/v1/Loan/GetById/{id}
        /// <summary>
        /// Get by id
        /// Method to get a loan by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetById/{id}")]
        public async Task<LoanResponse> GetById([FromRoute] int id)
        {
            var response = await _loanDomain.GetById(id);
            var result = _mapper.Map<Loan, LoanResponse>(response);
            return result;
        }
        
        /// GET: api/v1/Loan/GetLoanDetailsByDni?dni={dni}
        /// <summary>
        /// Get loan details by dni
        /// Method to get loan details(Nombre, Apellido Tipo Interes, Tasa de Interes, Periodo y Dia de pago), by dni
        /// </summary>
        /// <param name="dni"></param>
        /// <returns></returns>
        [HttpGet("GetLoanDetailsByDni")]
        public async Task<IActionResult> GetLoanDetailsByDni([FromQuery] string dni)
        {
            try
            {
                if (string.IsNullOrEmpty(dni))
                {
                    return BadRequest("El DNI es requerido.");
                }

                var loanDetails = await _loanDomain.GetLoanDetailsByDni(dni);

                if (loanDetails == null)
                {
                    return NotFound("No se encontraron detalles del préstamo para el DNI proporcionado.");
                }

                return Ok(loanDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
        
        /// POST: api/v1/Loan/Post
        /// <summary>
        /// Post
        /// Method to create a loan
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Post")]
        public async Task<IActionResult> Post([FromBody] LoanRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var loan = _mapper.Map<LoanRequest, Loan>(request);
                var createdLoan = await _loanDomain.Create(loan);

                if (createdLoan == null)
                {
                    return StatusCode(500, "A problem happened while handling your request.");
                }

                var loanResponse = _mapper.Map<Loan, LoanResponse>(createdLoan);
                return CreatedAtAction(nameof(GetById), new { id = loanResponse.Id }, loanResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    
        // PUT: api/v1/Loan/Put/{id}
        /// <summary>
        ///  Put
        ///  Method to update a loan
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("Put/{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] LoanRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var result = _mapper.Map<LoanRequest, Loan >(request);
                return Ok(await _loanDomain.Update(result, id));
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }

        /// DELETE: api/v1/Loan/Delete/{id}
        /// <summary>
        /// Delete
        /// Method to delete a loan
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Delete/{id}")]
        public async Task<bool> Delete(int id)
        {
            return await _loanDomain.Delete(id);
        }
    }
}