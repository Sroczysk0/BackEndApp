using ConsumerComplaints.Core.DTOs;
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

        public async Task<ActionResult<IEnumerable<ComplaintDto>>> GetAllComplaints()

        {

            var complaints = (await _repository.GetAllAsync()).Take(50).ToList();
 
            var result = complaints.Select(c => new ComplaintDto

            {

                Id = c.Id,

                Product = c.Product,

                Issue = c.Issue,

                Company = c.Company,

                DateReceived = c.DateReceived

            }).ToList();
 
            return Ok(result);

        }

    }

}