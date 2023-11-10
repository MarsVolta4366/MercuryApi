using AutoMapper;
using MercuryApi.Data.Dtos;
using MercuryApi.Data.Repository;
using MercuryApi.Data.Upserts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace MercuryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public UserController(IRepositoryManager repositoryManager, IMapper mapper, IConfiguration configuration)
        {
            _repositoryManager = repositoryManager;
            _mapper = mapper;
            _configuration = configuration;
        }

        [HttpGet("get-current-user"), Authorize]
        public async Task<ActionResult<UserDto?>> GetCurrentUser()
        {
            string userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            User? user = await _repositoryManager.User.GetUserById(int.Parse(userId), trackChanges: false);
            if (user == null)
            {
                return Unauthorized();
            }
            UserDto response = _mapper.Map<UserDto>(user);
            return Ok(response);
        }

        [HttpPost("sign-up")]
        public async Task<ActionResult<string>> CreateUser(UserUpsert request)
        {
            if (await _repositoryManager.User.GetUserByUsername(request.Username, false) != null)
            {
                return BadRequest("Username is already taken.");
            }
            User user = _mapper.Map<User>(request);

            // Hash password.
            string passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(user.Password, 13);
            user.Password = passwordHash;

            await _repositoryManager.User.CreateUser(user);
            await _repositoryManager.SaveAsync();

            return Ok(CreateToken(user));
        }

        [HttpGet("check-if-username-exists/{username}")]
        public async Task<ActionResult> CheckIfUsernameExists([FromRoute] string username)
        {
            if (await _repositoryManager.User.GetUserByUsername(username, false) != null)
            {
                return Ok(new { exists = true });
            }
            return Ok(new { exists = false });
        }

        [HttpPost("log-in")]
        public async Task<ActionResult<string>> LogIn(UserUpsert request)
        {
            User? user = await _repositoryManager.User.GetUserByUsername(request.Username, false);
            if (user == null || !BCrypt.Net.BCrypt.EnhancedVerify(request.Password, user.Password))
            {
                return Unauthorized("Failed to log in.");
            }
            return Ok(CreateToken(user));
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new()
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            };

            string signingKey = _configuration.GetSection("SigningKey").Value ?? throw new Exception("Coud not find signing key.");
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(signingKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            string jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }
    }
}
