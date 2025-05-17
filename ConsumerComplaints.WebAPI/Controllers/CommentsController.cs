using ConsumerComplaints.Core.Entities;
using ConsumerComplaints.Core.Interfaces;
using ConsumerComplaints.WebAPI.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ConsumerComplaints.Core.DTOs;
using ConsumerComplaints.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;

namespace ConsumerComplaints.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentRepository _repository;
        private readonly AppDbContext _context;
 
        public CommentsController(ICommentRepository repository, AppDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        [HttpGet("byComplaint/{complaintId}")]
        public async Task<IActionResult> GetByComplaint(int complaintId)
        {
            var comments = await _repository.GetByComplaintIdAsync(complaintId);

            var result = comments.Select(c => new CommentDto
            {
                Content = c.Content,
                ComplaintId = c.ComplaintId,
                AuthorName = c.User?.UserName ?? "anonim",
                CreatedAt = c.CreatedAt
            });

            return Ok(result);
        }

        [HttpPost]
        [Authorize] // wymuszamy token
        public async Task<IActionResult> PostComment([FromBody] CreateCommentDto dto, [FromServices] UserManager<UserEntity> userManager)
        {
            var exists = await _context.Complaints.AnyAsync(c => c.Id == dto.ComplaintId);
            if (!exists)
                return BadRequest("ComplaintId nie istnieje.");

            var comment = new Comment
            {
                Content = dto.Content,
                ComplaintId = dto.ComplaintId,
                CreatedAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
            };

            // Ręczne przypisanie UserId
            var user = await userManager.GetUserAsync(User);
            if (user != null)
            {
                comment.UserId = user.Id;
                Console.WriteLine("✅ Przypisano UserId z UserManager: " + user.Id);
            }

            await _repository.AddAsync(comment);
            await _repository.SaveChangesAsync();

            return CreatedAtAction(nameof(GetByComplaint), new { complaintId = comment.ComplaintId }, comment);
        }



        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] CommentDto dto)
        {
            var comment = await _repository.GetByIdAsync(id);
            if (comment == null) return NotFound();

            var loggedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (comment.UserId != loggedUserId)
                return Forbid("Nie jesteś autorem tego komentarza.");

            comment.Content = dto.Content;
            await _repository.UpdateAsync(comment);
            await _repository.SaveChangesAsync();

            return Ok(new { message = "Komentarz zaktualizowany", comment });
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _repository.GetByIdAsync(id);
            if (comment == null) return NotFound();

            var loggedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (comment.UserId != loggedUserId)
                return Forbid("Nie jesteś autorem tego komentarza.");

            await _repository.DeleteAsync(id);
            await _repository.SaveChangesAsync();

            return NoContent();
        }
    }
}
