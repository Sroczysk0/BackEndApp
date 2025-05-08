using ConsumerComplaints.Core.Entities;
using ConsumerComplaints.Core.Entities;
using ConsumerComplaints.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ConsumerComplaints.WebAPI.Dto;

namespace ConsumerComplaints.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentRepository _repository;

        public CommentsController(ICommentRepository repository)
        {
            _repository = repository;
        }

        // GET: /api/comments/byComplaint/5
        [HttpGet("byComplaint/{complaintId}")]
        public async Task<IActionResult> GetByComplaint(int complaintId)
        {
            var comments = await _repository.GetByComplaintIdAsync(complaintId);
            return Ok(comments);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> PostComment([FromBody] CommentDto dto)
        {
            var comment = new Comment
            {
                Content = dto.Content,
                ComplaintId = dto.ComplaintId,
                CreatedAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
            };

            if (User.Identity.IsAuthenticated)
            {
                comment.UserId = User.FindFirst("sub")?.Value;
            }

            await _repository.AddAsync(comment);
            await _repository.SaveChangesAsync();

            return CreatedAtAction(nameof(GetByComplaint), new { complaintId = comment.ComplaintId }, comment);
        }
    }
}
