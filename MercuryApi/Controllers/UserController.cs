using AutoMapper;
using MercuryApi.Data.Dtos;
using MercuryApi.Data.Repository;
using MercuryApi.Data.Upserts;
using Microsoft.AspNetCore.Mvc;

namespace MercuryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;

        public UserController(IRepositoryManager repositoryManager, IMapper mapper)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
        }

        [HttpGet("{userId}")]
        public async Task<ActionResult> GetUserById([FromRoute] int userId)
        {
            User? user = await _repositoryManager.User.GetUserById(userId, trackChanges: false);
            if (user == null)
            {
                return NotFound($"No user found with id {userId}");
            }
            UserDto userDto = _mapper.Map<UserDto>(user);
            return Ok(userDto);
        }

        [HttpPost]
        public async Task<ActionResult> CreateUser(UserUpsert userUpsert)
        {
            if (await _repositoryManager.User.GetUserByUsername(userUpsert.Username, false) != null)
            {
                return BadRequest("Username is already taken.");
            }
            User user = _mapper.Map<User>(userUpsert);

            // Hash password.
            string passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(user.Password, 13);
            user.Password = passwordHash;

            await _repositoryManager.User.CreateUser(user);
            await _repositoryManager.SaveAsync();

            UserDto userDto = _mapper.Map<UserDto>(user);
            return CreatedAtAction(nameof(GetUserById), new { userId = user.Id }, userDto);
        }

        //[HttpPost("login")]
        //public async Task<string> Login(UserUpsert userUpsert)
        //{
        //    User? user = await _repositoryManager.User.GetUserByUsername(userUpsert.Username, false);
        //    if (user != null && BCrypt.Net.BCrypt.EnhancedVerify(userUpsert.Password, user.Password))
        //    {
        //        return "Logged in";

        //    }
        //    return "Nope";
        //}
    }
}
