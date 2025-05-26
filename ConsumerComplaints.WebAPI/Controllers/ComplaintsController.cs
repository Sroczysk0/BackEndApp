using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ConsumerComplaints.Core.DTOs;
using ConsumerComplaints.Core.Entities;
using ConsumerComplaints.Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ConsumerComplaints.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComplaintsController : ControllerBase
    {
        private readonly IComplaintRepository _repository;

        public ComplaintsController(IComplaintRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ComplaintDto>>> GetAllComplaints(int page = 1, int pageSize = 20)
        {
            var complaints = (await _repository.GetAllAsync())
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            var result = complaints.Select(c => new ComplaintDto
            {
                Id = c.Id,
                Product = c.Product,
                Issue = c.Issue,
                DateReceived = c.DateReceived,
                State = c.State,
                SubIssue = c.SubIssue
            });

            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ComplaintDto>> GetComplaint(int id)
        {
            var complaint = await _repository.GetByIdAsync(id);
            if (complaint == null)
                return NotFound(new { message = $"Complaint o ID {id} nie istnieje." });

            return Ok(new ComplaintDto
            {
                Id = complaint.Id,
                Product = complaint.Product,
                Issue = complaint.Issue,
                DateReceived = complaint.DateReceived,
                State = complaint.State,
                SubIssue = complaint.SubIssue
            });
        }

        [HttpPost]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> CreateComplaint([FromBody] CreateComplaintDto dto)
        {
            var userId = User.Identity?.Name;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized("Nie udało się odczytać użytkownika z tokena.");

            var complaint = new Complaint
            {
                Product = dto.Product,
                Issue = dto.Issue,
                DateReceived = dto.DateReceived,
                State = dto.State,
                SubIssue = dto.SubIssue,
                SubmittedVia = dto.SubmittedVia,
                UserId = userId
            };

            await _repository.AddAsync(complaint);
            await _repository.SaveChangesAsync();

            return Ok(new
            {
                message = "✅ Dodano zgłoszenie",
                complaintId = complaint.Id
            });
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> UpdateComplaint(int id, [FromBody] ComplaintDto dto)
        {
            var complaint = await _repository.GetByIdAsync(id);
            if (complaint == null)
                return NotFound();

            var userId = User.Identity?.Name;
            var isAdmin = User.IsInRole("Admin");

            if (!isAdmin && complaint.UserId != userId)
                return Forbid("Nie możesz edytować tej skargi.");

            complaint.Product = dto.Product;
            complaint.Issue = dto.Issue;
            complaint.DateReceived = dto.DateReceived;
            complaint.State = dto.State;
            complaint.SubIssue = dto.SubIssue;

            await _repository.UpdateAsync(complaint);
            return Ok(new { message = "✅ Zaktualizowano skargę." });
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "User,Admin")]
        public async Task<IActionResult> DeleteComplaint(int id)
        {
            var complaint = await _repository.GetByIdAsync(id);
            if (complaint == null)
                return NotFound();

            var userId = User.Identity?.Name;
            var isAdmin = User.IsInRole("Admin");

            if (!isAdmin && complaint.UserId != userId)
                return Forbid("Nie możesz usunąć tej skargi.");

            await _repository.DeleteAsync(id);
            return Ok(new { message = $"✅ Usunięto skargę {id}" });
        }

        [HttpGet("ping-test")]
        [Authorize]
        public IActionResult PingTest()
        {
            return Ok(new
            {
                name = User.Identity?.Name,
                isAdmin = User.IsInRole("Admin")
            });
        }
        
        [HttpGet("test-noauth")]
        public IActionResult TestNoAuth()
        {
            return Ok("🟢 ComplaintsController działa!");
        }


    }
}
