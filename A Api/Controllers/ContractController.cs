using AutoMapper;
using Data.Model;
using Domain.Service.Impl;
using Microsoft.AspNetCore.Mvc;
using UniqueTrip.Request;
using UniqueTrip.Response;

namespace UniqueTrip.Controllers
{
    /// <summary>
    /// Contract Controller
    /// </summary>
    [ApiController]
    [Route("api/v1/Contract")]
    public class ContractController : Controller
    {
        private readonly IContractDomain _contractDomain;
        private readonly IMapper _mapper;
        
        /// <summary>
        ///  Constructor
        /// </summary>
        /// <param name="contractDomain"></param>
        /// <param name="mapper"></param>
        public ContractController(IContractDomain contractDomain, IMapper mapper)
        {
            _contractDomain = contractDomain;
            _mapper = mapper;

        }
        /// GET: api/v1/Contract/GetAll
        /// <summary>
        /// Get All
        /// Method to get all contracts
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<List<ContractResponse>> GetAll()
        {
            var response = await _contractDomain.GetAll();
            var result = _mapper.Map<List<Contract>, List<ContractResponse>>(response);
            return result;
        }
        
        /// GET: api/v1/Contract/GetById/{id}
        /// <summary>
        /// Get by id
        /// Method to get a contract by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetById/{id}")]
        public async Task<ContractResponse> GetById([FromRoute] int id)
        {
            var response = await _contractDomain.GetById(id);
            var result = _mapper.Map<Contract, ContractResponse>(response);
            return result;
        }
        
        /// GET: api/v1/Contract/GetByClientId/{id}
        /// <summary>
        /// Get by clientId
        /// Method to get a contract by clientId
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetByClientId/{id}")]
        public async Task<ContractResponse> GetByClientId([FromRoute] int id)
        {
            var response = await _contractDomain.GetByClientId(id);
            var result = _mapper.Map<Contract, ContractResponse>(response);
            return result;
        }
        
        /// GET: api/v1/Contract/GetByTypeRate
        /// <summary>
        /// Get by typeRate
        /// Method to get contracts by typeRate
        /// </summary>
        /// <param name="typeRate"></param>
        /// <returns></returns>
        [HttpGet("GetByTypeRate")]
        public async Task<List<ContractResponse>> GetByTypeRate([FromQuery] string typeRate)
        {
            Contract initial = new Contract()
            {
                TypeRate = typeRate,
            };
            var response = await _contractDomain.GetByTypeRate(initial);
            var result = _mapper.Map<List<Contract>, List<ContractResponse>>(response);
            return result;
        }
        
        /// GET: api/v1/Contract/GetByPenaltyTypeRate?typePenaltyRate={typePenaltyRate}
        /// <summary>
        /// Get by typePenaltyRate
        /// Method to get contracts by typePenaltyRate
        /// </summary>
        /// <param name="typePenaltyRate"></param>
        /// <returns></returns>
        [HttpGet("GetByPenaltyTypeRate")]
        public async Task<List<ContractResponse>> GetByTypePenaltyRate([FromQuery] string typePenaltyRate)
        {
            Contract initial = new Contract()
            {
                TypePenaltyRate = typePenaltyRate,
            };
            var response = await _contractDomain.GetByTypePenaltyRate(initial);
            var result = _mapper.Map<List<Contract>, List<ContractResponse>>(response);
            return result;
        }
        
        /// POST: api/v1/Contract/Post
        /// <summary>
        /// Post 
        /// Method to create a contract
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Post")]
        public async Task<IActionResult> Post([FromBody] ContractRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var result = _mapper.Map<ContractRequest, Contract>(request);
                return Ok(await _contractDomain.Create(result));
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }

        /// PUT: api/v1/Contract/Put/{id}
        /// <summary>
        /// Put
        /// Method to update a contract
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("Put/{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] ContractRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var result = _mapper.Map<ContractRequest, Contract>(request);
                return Ok(await _contractDomain.Update(result, id));
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }
        /// DELETE: api/v1/Contract/Delete/{id}
        /// <summary>
        /// Delete
        /// Method to delete a contract
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Delete/{id}")]
        public async Task<bool> Delete(int id)
        {
            return await _contractDomain.Delete(id);
        }
    }
}