using AutoMapper;
using Data.Model;
using Domain.Service.Impl;
using Microsoft.ApplicationInsights;
using Microsoft.AspNetCore.Mvc;
using UniqueTrip.Request;
using UniqueTrip.Response;

namespace UniqueTrip.Controllers
{
    /// <summary>
    /// User Controller
    /// </summary>
    [ApiController]
    [Route("api/v1/User")]
    public class UserController : Controller
    {
        private readonly IUserDomain _userDomain;
        private readonly IMapper _mapper;

        /// <summary>
        ///  Constructor
        /// </summary>
        /// <param name="userDomain"></param>
        /// <param name="mapper"></param>
        public UserController(IUserDomain userDomain, IMapper mapper)
        {
            _userDomain = userDomain;
            _mapper = mapper;

        }
        /// GET: api/v1/User/GetAll
        /// <summary>
        ///  Get All
        ///  Method to get all users
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAll")]
        public async Task<List<UserResponse>> GetAll()
        {
            var response = await _userDomain.GetAll();
            var result = _mapper.Map<List<User>, List<UserResponse>>(response);
            return result;
        } 
        
        
        /// GET: api/v1/User/GetById/{id}
        /// <summary>
        ///  Get by id
        ///  Method to get a user by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("GetById/{id}")]
        public async Task<UserResponse> GetById([FromRoute] int id)
        {
            var response = await _userDomain.GetById(id);
            var result = _mapper.Map<User, UserResponse>(response);
            return result;
        }

        /// GET: api/v1/User/GetByEmail?email={email}
        /// <summary>
        ///  Get by email
        ///  Method to get a user by email
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        [HttpGet("GetByEmail")]
        public async Task<UserResponse> GetByEmail([FromQuery] string email)
        {
            User initial = new User()
            {
                Email = email
            };
            var response = await _userDomain.GetByEmail(initial);
            var result = _mapper.Map<User, UserResponse>(response);
            return result;
        }
        
        /// POST: api/v1/User/Login
        /// <summary>
        ///  Login
        ///  Method to Login a user
        /// </summary>
        /// <param name="userInput"></param>
        /// <returns></returns>
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserRequest userInput)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var result = _mapper.Map<UserRequest, User>(userInput);
                return Ok(await _userDomain.Login(result));
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }
        
        /// POST: api/v1/User/Register
        /// <summary>
        ///  Register
        ///  Method to register a user
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var result = _mapper.Map<UserRequest, User>(request);
                return Ok(await _userDomain.Register(result));
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }
        
        /// PUT: api/v1/User/Put/{id}
        /// <summary>
        ///  Put
        ///  Method to update a user
        /// </summary>
        /// <param name="id"></param>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut("Put/{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] UserRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest();
                }

                var tourist = _mapper.Map<UserRequest, User>(request);
                return Ok(await _userDomain.Update(tourist, id));
            }
            catch (Exception e)
            {
                return StatusCode(500);
            }
        }

        /// DELETE: api/v1/User/Delete/{id}
        /// <summary>
        ///  Delete
        ///  Method to delete a user isActive
        ///  Set isActive to false but not delete the user also you can't use it
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("Delete/{id}")]
        public async Task<bool> Delete(int id)
        {
            return await _userDomain.Delete(id);
        }
    }
}