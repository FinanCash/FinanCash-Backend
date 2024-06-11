using AutoMapper;
using Data.Model;
using Domain.Service.Impl;
using Microsoft.AspNetCore.Mvc;
using UniqueTrip.Request;
using UniqueTrip.Response;

namespace UniqueTrip.Controllers
{
    /// <summary>
    /// Client Controller
    /// </summary>
    [ApiController]
    [Route("api/v1/Client")]
    public class ClientController : Controller
    {
        private readonly IClientDomain _clientDomain;
        private readonly IMapper _mapper;
        
        /// <summary>
        ///  Constructor
        /// </summary>
        /// <param name="clientDomain"></param>
        /// <param name="mapper"></param>
        public ClientController(IClientDomain clientDomain, IMapper mapper)
        {
            _clientDomain = clientDomain;
            _mapper = mapper;

        }
        /// GET: api/v1/Client/GetAll
        /// <summary>
        ///  Get All
        ///  Method to get all clients
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<List<ClientResponse>> GetAll()
        {
            var response = await _clientDomain.GetAll();
            var result = _mapper.Map<List<Client>, List<ClientResponse>>(response);
            return result;
        }

        /// GET: api/v1/Client/GetById/{id}
        /// <summary>
        ///  Get by id
        ///  Method to get a client by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetById/{id}")]
        public async Task<ClientResponse> GetById([FromRoute] int id)
        {
            var response = await _clientDomain.GetById(id);
            var result = _mapper.Map<Client, ClientResponse>(response);
            return result;
        }

        /// GET: api/v1/Client/GetByDni?dni={dni}
        /// <summary>
        ///  Get by DNI
        /// Method to get a client by DNI
        /// </summary>
        /// <param name="dni"></param>
        /// <returns></returns>
        [HttpGet("GetByDni")]
        public async Task<ClientResponse> GetByDni([FromQuery] string dni)
        {
            Client initial = new Client()
            {
                Dni = dni,
            };
            var response = await _clientDomain.GetByDni(initial.Dni);
            var result = _mapper.Map<Client, ClientResponse>(response);
            return result;
        }
        /// POST: api/v1/Client/Post
        /// <summary>
        ///  Post
        ///  Method to create a client
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Post")]
        public async Task<IActionResult> Post([FromBody] ClientRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var result = _mapper.Map<ClientRequest, Client>(request);
                return Ok(await _clientDomain.Create(result));
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }

        /// PUT: api/v1/Client/Put/{id}
        /// <summary>
        ///  Put
        ///  Method to update a client
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("Put/{id}")]
        public async Task<IActionResult> Put([FromRoute] int id, [FromBody] ClientRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var result = _mapper.Map<ClientRequest, Client >(request);
                return Ok(await _clientDomain.Update(result, id));
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }

        /// DELETE: api/v1/Client/Delete/{id}
        /// <summary>
        ///  Delete
        ///  Method to delete a client by isActive
        ///  Set isActive to false but not delete the client also you can't use it
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Delete/{id}")]
        public async Task<bool> Delete(int id)
        {
            return await _clientDomain.Delete(id);
        }
    }
}