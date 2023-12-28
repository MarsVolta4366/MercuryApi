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

        [HttpPut]
        public async Task<ActionResult> UpdateComment([FromBody] CommentUpsert request)
        {
            int userId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            CommentDto? response = await _commentBusinessLogic.UpdateComment(request, userId);

            if (response == null) return NotFound();
            return Ok(response);
        }

        [HttpDelete("{commentId}")]
        public async Task<ActionResult> DeleteComment([FromRoute] int commentId)
        {
            int userId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            await _commentBusinessLogic.DeleteComment(commentId, userId);

            return NoContent();
        }
    }
}
