using MercuryApi.BLL;
using MercuryApi.Data.Dtos;
using MercuryApi.Data.Upserts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace MercuryApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly ICommentBusinessLogic _commentBusinessLogic;

        public CommentController(ICommentBusinessLogic commentBusinessLogic)
        {
            _commentBusinessLogic = commentBusinessLogic;
        }

        [HttpPost]
        public async Task<ActionResult> CreateComment([FromBody] CommentUpsert request)
        {
            request.UserId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            CommentDto response = await _commentBusinessLogic.CreateComment(request);
            return Ok(response);
        }
    }
}
