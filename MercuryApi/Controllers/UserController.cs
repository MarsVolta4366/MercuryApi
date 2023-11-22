using MercuryApi.BLL;
using MercuryApi.Data.Dtos;
using MercuryApi.Data.Upserts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MercuryApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBusinessLogic _userBusinessLogic;

        public UserController(IUserBusinessLogic userBusinessLogic)
        {
            _userBusinessLogic = userBusinessLogic;
        }

        [HttpGet("get-current-user"), Authorize]
        public async Task<ActionResult<UserDto?>> GetCurrentUser()
        {
            string userId = HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
            UserDto? response = await _userBusinessLogic.GetUserById(int.Parse(userId));
            if (response == null) return Unauthorized();
            return Ok(response);
        }

        [HttpPost("sign-up")]
        public async Task<ActionResult<string>> SignUp(UserUpsert request)
        {
            string? token = await _userBusinessLogic.SignUp(request);
            if (token == null) return BadRequest("Username is already taken.");
            return Ok(token);
        }

        [HttpGet("get-user-by-username/{username}")]
        public async Task<ActionResult> GetUserByUsername([FromRoute] string username)
        {
            UserDto? response = await _userBusinessLogic.GetUserByUsername(username);
            return Ok(response);
        }

        [HttpGet("get-user-by-username-and-team-id/{username}/{teamId}")]
        public async Task<ActionResult> GetUserByUsernameAndTeamId([FromRoute] string username, [FromRoute] int teamId)
        {
            UserDto? response = await _userBusinessLogic.GetUserByUsernameAndTeamId(username, teamId);
            return Ok(response);
        }

        [HttpPost("log-in")]
        public async Task<ActionResult<string>> LogIn(UserUpsert request)
        {
            string? token = await _userBusinessLogic.LogIn(request);
            if (token == null)
            {
                return Unauthorized("Failed to log in.");
            }
            return Ok(token);
        }
    }
}
