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

        [HttpGet("{id}")]
        public async Task<ActionResult<ComplaintDto>> GetComplaint(int id)
        {
            var complaint = await _repository.GetByIdAsync(id);
            if (complaint == null) return NotFound();

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
        public async Task<IActionResult> CreateComplaint([FromBody] ComplaintDto dto)
        {
            var complaint = new Complaint
            {
                Product = dto.Product,
                Issue = dto.Issue,
                DateReceived = dto.DateReceived,
                State = dto.State,
                SubIssue = dto.SubIssue
            };

            await _repository.AddAsync(complaint);
            return CreatedAtAction(nameof(GetComplaint), new { id = complaint.Id }, complaint);
        }

        [HttpPut("{id}")]
        [AllowAnonymous] // 👈 tymczasowo, do testów
        public async Task<IActionResult> UpdateComplaint(int id, [FromBody] ComplaintDto dto)
        {
            Console.WriteLine($"Próba edycji skargi ID: {id}");
            var complaint = await _repository.GetByIdAsync(id);
            if (complaint == null)
            {
                Console.WriteLine("❌ Nie znaleziono skargi o podanym ID.");
                return NotFound(new { message = "Complaint not found" });
            }

            complaint.Product = dto.Product;
            complaint.Issue = dto.Issue;
            complaint.DateReceived = dto.DateReceived;
            complaint.State = dto.State;
            complaint.SubIssue = dto.SubIssue;

            await _repository.UpdateAsync(complaint);
            Console.WriteLine("✅ Skarga została zaktualizowana.");

            return Ok(new { message = "Complaint updated successfully", complaint });
        }


        [HttpDelete("{id}")]
        [AllowAnonymous] // 👈 tymczasowo, do testów
        public async Task<IActionResult> DeleteComplaint(int id)
        {
            Console.WriteLine($"🔁 Próba usunięcia skargi ID: {id}");
            await _repository.DeleteAsync(id);
            Console.WriteLine($"✅ Próba zakończona.");
            return NoContent();
        }

    }
}
