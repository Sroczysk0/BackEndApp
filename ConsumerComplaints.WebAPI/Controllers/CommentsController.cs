using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ConsumerComplaints.Core.DTOs;
using ConsumerComplaints.Core.Entities;
using ConsumerComplaints.Core.Interfaces;
using ConsumerComplaints.Infrastructure.Persistence;
using ConsumerComplaints.WebAPI.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> PostComment([FromBody] CreateCommentDto dto)
        {
            var exists = await _context.Complaints.AnyAsync(c => c.Id == dto.ComplaintId);
            if (!exists)
                return BadRequest("ComplaintId nie istnieje.");

            var userId = User.Identity != null && User.Identity.IsAuthenticated
                ? User.Identity?.Name
                : null;

            var comment = new Comment
            {
                Content = dto.Content,
                ComplaintId = dto.ComplaintId,
                CreatedAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                UserId = userId // może być null dla anonimów
            };

            await _repository.AddAsync(comment);
            await _repository.SaveChangesAsync();

            return CreatedAtAction(nameof(GetByComplaint), new { complaintId = comment.ComplaintId }, comment);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] CommentDto dto)
        {
            var comment = await _repository.GetByIdAsync(id);
            if (comment == null)
                return NotFound();

            var loggedUserId = User.Identity?.Name;
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
            if (comment == null)
                return NotFound();

            var loggedUserId = User.Identity?.Name;
            if (comment.UserId != loggedUserId)
                return Forbid("Nie jesteś autorem tego komentarza.");

            await _repository.DeleteAsync(id);
            await _repository.SaveChangesAsync();

            return NoContent();
        }
    }
}
