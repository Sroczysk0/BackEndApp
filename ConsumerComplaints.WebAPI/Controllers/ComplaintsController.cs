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
    /// <summary>
    /// Kontroler do zarządzania zgłoszeniami (complaints).
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class ComplaintsController : ControllerBase
    {
        private readonly IComplaintRepository _repository;

        public ComplaintsController(IComplaintRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Zwraca wszystkie zgłoszenia z paginacją i linkami HATEOAS.
        /// </summary>
        /// <param name="page">Numer strony (domyślnie 1)</param>
        /// <param name="pageSize">Liczba wyników na stronie (domyślnie 20)</param>
        /// <returns>Lista zgłoszeń</returns>
        [HttpGet]
        public async Task<ActionResult> GetAllComplaints([FromQuery] int page = 1, [FromQuery] int pageSize = 20)
        {
            var all = await _repository.GetAllAsync();
            var totalItems = all.Count();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var paged = all.Skip((page - 1) * pageSize).Take(pageSize);

            var result = paged.Select(c => new ComplaintDto
            {
                Id = c.Id,
                Product = c.Product,
                Issue = c.Issue,
                DateReceived = c.DateReceived,
                State = c.State,
                SubIssue = c.SubIssue
            });

            var links = new Dictionary<string, string>
            {
                ["first"] = Url.Action(nameof(GetAllComplaints), null, new { page = 1, pageSize }, Request.Scheme)!,
                ["last"] = Url.Action(nameof(GetAllComplaints), null, new { page = totalPages, pageSize }, Request.Scheme)!
            };

            if (page > 1)
                links["prev"] = Url.Action(nameof(GetAllComplaints), null, new { page = page - 1, pageSize }, Request.Scheme)!;

            if (page < totalPages)
                links["next"] = Url.Action(nameof(GetAllComplaints), null, new { page = page + 1, pageSize }, Request.Scheme)!;

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
        /// Zwraca szczegóły zgłoszenia o podanym ID.
        /// </summary>
        /// <param name="id">ID zgłoszenia</param>
        /// <returns>Obiekt zgłoszenia</returns>
        /// <response code="200">Znaleziono zgłoszenie</response>
        /// <response code="404">Nie znaleziono zgłoszenia</response>
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

        /// <summary>
        /// Tworzy nowe zgłoszenie. Tylko dla roli Admin.
        /// </summary>
        /// <param name="dto">Dane zgłoszenia</param>
        /// <returns>ID nowego zgłoszenia</returns>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateComplaint([FromBody] CreateComplaintDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

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
                message = "Dodano zgłoszenie",
                complaintId = complaint.Id
            });
        }

        
        /// <summary>
        /// Aktualizuje istniejące zgłoszenie. Tylko dla roli Admin.
        /// </summary>
        /// <param name="id">ID zgłoszenia</param>
        /// <param name="dto">Zaktualizowane dane</param>
        /// <returns>Status aktualizacji</returns>
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateComplaint(int id, [FromBody] ComplaintDto dto)
        {
            var complaint = await _repository.GetByIdAsync(id);
            if (complaint == null)
                return NotFound();

            complaint.Product = dto.Product;
            complaint.Issue = dto.Issue;
            complaint.DateReceived = dto.DateReceived;
            complaint.State = dto.State;
            complaint.SubIssue = dto.SubIssue;

            await _repository.UpdateAsync(complaint);
            return Ok(new { message = "Zaktualizowano skargę." });
        }

        /// <summary>
        /// Usuwa zgłoszenie. Tylko dla roli Admin.
        /// </summary>
        /// <param name="id">ID zgłoszenia</param>
        /// <returns>Status usunięcia</returns>
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteComplaint(int id)
        {
            var complaint = await _repository.GetByIdAsync(id);
            if (complaint == null)
                return NotFound();

            await _repository.DeleteAsync(id);
            return Ok(new { message = $"Usunięto skargę {id}" });
        }

        /// <summary>
        /// Zwraca posortowaną listę zgłoszeń po dacie lub stanie.
        /// </summary>
        /// <param name="sortBy">Pole do sortowania (date/state)</param>
        /// <param name="order">Kierunek sortowania (asc/desc)</param>
        /// <param name="page">Numer strony</param>
        /// <param name="pageSize">Liczba wyników na stronie</param>
        /// <returns>Lista posortowanych zgłoszeń</returns>
        [HttpGet("sorted")]
        public async Task<IActionResult> GetSortedComplaints(
            [FromQuery] string sortBy = "date",
            [FromQuery] string order = "asc",
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            var complaints = await _repository.GetAllAsync();

            var sorted = sortBy.ToLower() switch
            {
                "state" => order.ToLower() == "desc"
                    ? complaints.OrderByDescending(c => c.State)
                    : complaints.OrderBy(c => c.State),

                _ => order.ToLower() == "desc"
                    ? complaints.OrderByDescending(c => c.DateReceived)
                    : complaints.OrderBy(c => c.DateReceived)
            };

            var totalItems = sorted.Count();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var paged = sorted.Skip((page - 1) * pageSize).Take(pageSize);

            var result = paged.Select(c => new ComplaintDto
            {
                Id = c.Id,
                Product = c.Product,
                Issue = c.Issue,
                DateReceived = c.DateReceived,
                State = c.State,
                SubIssue = c.SubIssue
            });

            var links = new Dictionary<string, string>
            {
                ["first"] = Url.Action(nameof(GetSortedComplaints), null, new { sortBy, order, page = 1, pageSize }, Request.Scheme)!,
                ["last"] = Url.Action(nameof(GetSortedComplaints), null, new { sortBy, order, page = totalPages, pageSize }, Request.Scheme)!
            };

            if (page > 1)
            {
                links["prev"] = Url.Action(nameof(GetSortedComplaints), null, new { sortBy, order, page = page - 1, pageSize }, Request.Scheme)!;
            }

            if (page < totalPages)
            {
                links["next"] = Url.Action(nameof(GetSortedComplaints), null, new { sortBy, order, page = page + 1, pageSize }, Request.Scheme)!;
            }

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
        /// Zwraca zgłoszenia, których pole "Issue" zaczyna się od danego prefixu.
        /// </summary>
        /// <param name="prefix">Prefix pola Issue</param>
        /// <param name="page">Numer strony</param>
        /// <param name="pageSize">Liczba wyników</param>
        /// <returns>Przefiltrowane zgłoszenia</returns>
        [HttpGet("by-issue-prefix")]
        public async Task<IActionResult> GetByIssuePrefix(
            [FromQuery] string prefix,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            if (string.IsNullOrWhiteSpace(prefix))
                return BadRequest("Prefix nie może być pusty.");

            var filtered = (await _repository.GetAllAsync())
                .Where(c => c.Issue != null && c.Issue.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));

            var totalItems = filtered.Count();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var paged = filtered.Skip((page - 1) * pageSize).Take(pageSize);

            var result = paged.Select(c => new ComplaintDto
            {
                Id = c.Id,
                Product = c.Product,
                Issue = c.Issue,
                DateReceived = c.DateReceived,
                State = c.State,
                SubIssue = c.SubIssue
            });

            var links = new Dictionary<string, string>
            {
                ["first"] = Url.Action(nameof(GetByIssuePrefix), null, new { prefix, page = 1, pageSize }, Request.Scheme)!,
                ["last"] = Url.Action(nameof(GetByIssuePrefix), null, new { prefix, page = totalPages, pageSize }, Request.Scheme)!
            };

            if (page > 1)
            {
                links["prev"] = Url.Action(nameof(GetByIssuePrefix), null, new { prefix, page = page - 1, pageSize }, Request.Scheme)!;
            }

            if (page < totalPages)
            {
                links["next"] = Url.Action(nameof(GetByIssuePrefix), null, new { prefix, page = page + 1, pageSize }, Request.Scheme)!;
            }

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
        /// Zwraca zgłoszenia, których pole "SubIssue" zaczyna się od danego prefixu.
        /// </summary>
        /// <param name="prefix">Prefix pola SubIssue</param>
        /// <param name="page">Numer strony</param>
        /// <param name="pageSize">Liczba wyników</param>
        /// <returns>Przefiltrowane zgłoszenia</returns>
        [HttpGet("by-subissue-prefix")]
        public async Task<IActionResult> GetBySubIssuePrefix(
            [FromQuery] string prefix,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 20)
        {
            if (string.IsNullOrWhiteSpace(prefix))
                return BadRequest("Prefix nie może być pusty.");

            var filtered = (await _repository.GetAllAsync())
                .Where(c => c.SubIssue != null && c.SubIssue.StartsWith(prefix, StringComparison.OrdinalIgnoreCase));

            var totalItems = filtered.Count();
            var totalPages = (int)Math.Ceiling((double)totalItems / pageSize);

            var paged = filtered.Skip((page - 1) * pageSize).Take(pageSize);

            var result = paged.Select(c => new ComplaintDto
            {
                Id = c.Id,
                Product = c.Product,
                Issue = c.Issue,
                DateReceived = c.DateReceived,
                State = c.State,
                SubIssue = c.SubIssue
            });

            var links = new Dictionary<string, string>
            {
                ["first"] = Url.Action(nameof(GetBySubIssuePrefix), null, new { prefix, page = 1, pageSize }, Request.Scheme)!,
                ["last"] = Url.Action(nameof(GetBySubIssuePrefix), null, new { prefix, page = totalPages, pageSize }, Request.Scheme)!
            };

            if (page > 1)
            {
                links["prev"] = Url.Action(nameof(GetBySubIssuePrefix), null, new { prefix, page = page - 1, pageSize }, Request.Scheme)!;
            }

            if (page < totalPages)
            {
                links["next"] = Url.Action(nameof(GetBySubIssuePrefix), null, new { prefix, page = page + 1, pageSize }, Request.Scheme)!;
            }

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



        
    }
}
