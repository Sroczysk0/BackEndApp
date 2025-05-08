using ConsumerComplaints.Core.DTOs;
using ConsumerComplaints.Core.Entities;
using CustomerComplaints.Core.Interfaces;
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

        // GET: /api/complaints
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ComplaintDto>>> GetAllComplaints(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
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

        [HttpGet("sorted")]
        public async Task<ActionResult<IEnumerable<ComplaintDto>>> GetSortedComplaints(
            [FromQuery] string primary = "date",
            [FromQuery] string primaryOrder = "asc",
            [FromQuery] string secondary = "state",
            [FromQuery] string secondaryOrder = "asc",
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var complaints = await _repository.GetAllAsync();

            IOrderedEnumerable<Complaint> sorted = (primary.ToLower(), primaryOrder.ToLower()) switch
            {
                ("state", "desc") => complaints.OrderByDescending(c => c.State),
                ("state", _) => complaints.OrderBy(c => c.State),
                (_, "desc") => complaints.OrderByDescending(c => c.DateReceived),
                _ => complaints.OrderBy(c => c.DateReceived),
            };

            sorted = (secondary.ToLower(), secondaryOrder.ToLower()) switch
            {
                ("state", "desc") when secondary != primary => sorted.ThenByDescending(c => c.State),
                ("state", _) when secondary != primary => sorted.ThenBy(c => c.State),
                ("date", "desc") when secondary != primary => sorted.ThenByDescending(c => c.DateReceived),
                ("date", _) when secondary != primary => sorted.ThenBy(c => c.DateReceived),
                _ => sorted
            };

            var result = sorted
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new ComplaintDto
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

        // GET: /api/complaints/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ComplaintDto>> GetComplaint(int id)
        {
            var complaint = await _repository.GetByIdAsync(id);
            if (complaint == null)
                return NotFound();

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

        [HttpGet("by-issue-prefix")]
        public async Task<ActionResult<IEnumerable<ComplaintDto>>> GetByIssuePrefix(
            [FromQuery] string prefix,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            if (string.IsNullOrWhiteSpace(prefix))
                return BadRequest("Prefix is required.");

            var complaints = await _repository.GetAllAsync();

            var filtered = complaints
                .Where(c => !string.IsNullOrEmpty(c.Issue) && c.Issue.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new ComplaintDto
                {
                    Id = c.Id,
                    Product = c.Product,
                    Issue = c.Issue,
                    DateReceived = c.DateReceived,
                    State = c.State,
                    SubIssue = c.SubIssue
                });

            return Ok(filtered);
        }

        [HttpGet("by-subissue-prefix")]
        public async Task<ActionResult<IEnumerable<ComplaintDto>>> GetBySubIssuePrefix(
            [FromQuery] string prefix,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            if (string.IsNullOrWhiteSpace(prefix))
                return BadRequest("Prefix is required.");

            var complaints = await _repository.GetAllAsync();

            var filtered = complaints
                .Where(c => !string.IsNullOrEmpty(c.SubIssue) && c.SubIssue.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(c => new ComplaintDto
                {
                    Id = c.Id,
                    Product = c.Product,
                    Issue = c.Issue,
                    DateReceived = c.DateReceived,
                    State = c.State,
                    SubIssue = c.SubIssue
                });

            return Ok(filtered);
        }
    }
}
