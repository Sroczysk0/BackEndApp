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
    /// <summary>
    /// Kontroler odpowiedzialny za operacje na komentarzach.
    /// </summary>
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
        
        /// <summary>
        /// Zwraca komentarze przypisane do zgłoszenia (ComplaintId) z paginacją i linkami HATEOAS.
        /// </summary>
        /// <param name="complaintId">ID zgłoszenia</param>
        /// <param name="page">Numer strony (domyślnie 1)</param>
        /// <param name="pageSize">Liczba wyników na stronie (domyślnie 20)</param>
        /// <returns>Lista komentarzy z metadanymi i linkami</returns>
        [HttpGet("byComplaint/{complaintId}")]
        public async Task<IActionResult> GetByComplaint(int complaintId, [FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var comments = (await _repository.GetByComplaintIdAsync(complaintId)).ToList();
            var totalItems = comments.Count;
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var paged = comments.Skip((page - 1) * pageSize).Take(pageSize);

            var result = paged.Select(c => new CommentDto
            {
                CommentId = c.Id,
                Content = c.Content,
                ComplaintId = c.ComplaintId,
                AuthorName = c.User?.UserName ?? "anonim",
                CreatedAt = c.CreatedAt
            });

            var links = new Dictionary<string, string>
            {
                ["first"] = Url.Action(nameof(GetByComplaint), null, new { complaintId, page = 1, pageSize }, Request.Scheme)!,
                ["last"] = Url.Action(nameof(GetByComplaint), null, new { complaintId, page = totalPages, pageSize }, Request.Scheme)!
            };

            if (page > 1)
                links["prev"] = Url.Action(nameof(GetByComplaint), null, new { complaintId, page = page - 1, pageSize }, Request.Scheme)!;

            if (page < totalPages)
                links["next"] = Url.Action(nameof(GetByComplaint), null, new { complaintId, page = page + 1, pageSize }, Request.Scheme)!;

            return Ok(new
            {
                page,
                pageSize,
                totalItems,
                totalPages,
                data = result,
                links
            });
        }

        
        /// <summary>
        /// Dodaje nowy komentarz (anonimowy lub zalogowanego użytkownika).
        /// </summary>
        /// <param name="dto">Treść komentarza oraz ComplaintId</param>
        /// <returns>Stworzony komentarz</returns>
        /// <response code="201">Komentarz dodany</response>
        /// <response code="400">Nieprawidłowy ComplaintId</response>
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> PostComment([FromBody] CreateCommentDto dto)
        {
            var exists = await _context.Complaints.AnyAsync(c => c.Id == dto.ComplaintId);
            if (!exists)
                return BadRequest("ComplaintId nie istnieje.");

            var userId = User.Identity != null && User.Identity.IsAuthenticated
                ? User.FindFirstValue(ClaimTypes.NameIdentifier)
                : null;

            var comment = new Comment
            {
                Content = dto.Content,
                ComplaintId = dto.ComplaintId,
                CreatedAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                UserId = userId 
            };

            await _repository.AddAsync(comment);
            await _repository.SaveChangesAsync();

            return CreatedAtAction(nameof(GetByComplaint), new { complaintId = comment.ComplaintId }, comment);
        }
        /// <summary>
        /// Aktualizuje treść komentarza (tylko autora lub admina).
        /// </summary>
        /// <param name="id">ID komentarza</param>
        /// <param name="newContent">Nowa treść komentarza</param>
        /// <returns>Potwierdzenie edycji</returns>
        /// <response code="200">Komentarz zaktualizowany</response>
        /// <response code="403">Brak dostępu</response>
        /// <response code="404">Komentarz nie istnieje</response>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] string newContent)
        {
            var comment = await _repository.GetByIdAsync(id);
            if (comment == null)
                return NotFound();

            var loggedUserId = User.Identity?.Name;
            var isAdmin = User.IsInRole("Admin");

            if (!isAdmin && comment.UserId != loggedUserId)
                return Forbid("Nie jesteś autorem tego komentarza.");

            comment.Content = newContent;
            await _repository.UpdateAsync(comment);
            await _repository.SaveChangesAsync();

            return Ok(new { message = "Komentarz zaktualizowany.", content = newContent });
        }

        /// <summary>
        /// Usuwa komentarz (tylko autora lub admina).
        /// </summary>
        /// <param name="id">ID komentarza</param>
        /// <returns>Status 204 jeśli sukces</returns>
        /// <response code="204">Komentarz usunięty</response>
        /// <response code="403">Brak dostępu</response>
        /// <response code="404">Komentarz nie istnieje</response>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var comment = await _repository.GetByIdAsync(id);
            if (comment == null)
                return NotFound();

            var loggedUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var isAdmin = User.IsInRole("Admin");

            if (!isAdmin && comment.UserId != loggedUserId)
                return Forbid("Nie jesteś autorem tego komentarza.");

            await _repository.DeleteAsync(id);
            await _repository.SaveChangesAsync();

            return NoContent();
        }
    }
}
